using System.Net.Mime;
using FreshGuard.ColdTrack.Platform.Iam.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Resources;
using FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest;

[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthenticationController(IUserCommandService service) : ControllerBase
{
    [HttpPost("sign-up")]
    [SwaggerOperation(Summary = "Register a ColdTrack user account")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    public async Task<IActionResult> SignUp(SignUpResource resource, CancellationToken cancellationToken)
    {
        var result = await service.Handle(
            new SignUpCommand(resource.FullName, resource.Email, resource.Password, resource.Role), cancellationToken);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(UsersController.GetCurrentUser), "Users", null,
                UserResourceFromEntityAssembler.ToResource(result.Value!));

        return result.Error is IamError.EmailAlreadyRegistered
            ? Problem(statusCode: StatusCodes.Status409Conflict, title: result.Error.ToString(), detail: result.Message)
            : Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error?.ToString(), detail: result.Message);
    }

    [HttpPost("sign-in")]
    [SwaggerOperation(Summary = "Authenticate a ColdTrack user")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignIn(SignInResource resource, CancellationToken cancellationToken)
    {
        var result = await service.Handle(new SignInCommand(resource.Email, resource.Password), cancellationToken);
        if (result.IsFailure)
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: result.Error?.ToString(), detail: result.Message);

        var authenticated = result.Value;
        return Ok(new AuthenticatedUserResource(authenticated.Token,
            UserResourceFromEntityAssembler.ToResource(authenticated.User)));
    }
}


