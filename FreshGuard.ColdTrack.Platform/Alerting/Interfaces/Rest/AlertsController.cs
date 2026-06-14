using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using FreshGuard.ColdTrack.Platform.Alerting.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Alerting.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Rest.Resources;
using FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Rest.Transform;
using FreshGuard.ColdTrack.Platform.Shared.Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Rest;

[ApiController, Authorize, Route("api/v1/[controller]"), Produces(MediaTypeNames.Application.Json)]
public class AlertsController(IAlertCommandService commandService, IAlertQueryService queryService) : ControllerBase
{
    [HttpGet, SwaggerOperation(Summary = "List alerts with optional status and severity filters")]
    public async Task<IActionResult> GetAll([FromQuery] AlertStatus? status, [FromQuery] AlertSeverity? severity,
        CancellationToken cancellationToken) =>
        Ok((await queryService.Handle(new GetAlertsQuery(status, severity), cancellationToken))
            .Select(AlertResourceAssembler.ToResource));

    [HttpPatch("{alertId:int}/acknowledgment"), Authorize(Roles = "LogisticsAdmin")]
    [SwaggerOperation(Summary = "Acknowledge a triggered alert")]
    public async Task<IActionResult> Acknowledge(int alertId, CancellationToken cancellationToken) =>
        ToActionResult(await commandService.Handle(new AcknowledgeAlertCommand(alertId), cancellationToken));

    [HttpPatch("{alertId:int}/resolution"), Authorize(Roles = "LogisticsAdmin")]
    [SwaggerOperation(Summary = "Resolve an alert")]
    public async Task<IActionResult> Resolve(int alertId, ResolveAlertResource resource,
        CancellationToken cancellationToken)
    {
        var subject = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(subject, out var userId)) return Unauthorized();
        return ToActionResult(await commandService.Handle(new ResolveAlertCommand(alertId, userId, resource.Notes),
            cancellationToken));
    }

    private IActionResult ToActionResult(Result<Alert> result)
    {
        if (result.IsSuccess) return Ok(AlertResourceAssembler.ToResource(result.Value!));
        var status = Equals(result.Error, AlertingError.AlertNotFound)
            ? StatusCodes.Status404NotFound
            : StatusCodes.Status409Conflict;
        return Problem(statusCode: status, title: result.Error?.ToString(), detail: result.Message);
    }
}
