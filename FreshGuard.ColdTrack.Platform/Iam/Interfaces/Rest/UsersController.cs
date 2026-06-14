using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Resources;
using FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(IUserAccountRepository repository) : ControllerBase
{
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Get the authenticated user")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var subject = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(subject, out var userId)) return Unauthorized();
        var user = await repository.FindByIdAsync(userId, cancellationToken);
        return user is null ? NotFound() : Ok(UserResourceFromEntityAssembler.ToResource(user));
    }
}
