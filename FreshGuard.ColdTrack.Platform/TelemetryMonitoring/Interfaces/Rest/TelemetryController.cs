using System.Net.Mime;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest.Resources;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest;

[ApiController]
[Authorize]
[Route("api/v1")]
[Produces(MediaTypeNames.Application.Json)]
public class TelemetryController(ITelemetryCommandService commandService, ITelemetryQueryService queryService)
    : ControllerBase
{
    [HttpPost("telemetry")]
    [Authorize(Roles = "LogisticsAdmin,Driver")]
    [SwaggerOperation(Summary = "Record a sensor telemetry reading")]
    public async Task<IActionResult> Record(RecordTelemetryResource resource, CancellationToken cancellationToken)
    {
        var recordedAt = resource.RecordedAt == default ? DateTimeOffset.UtcNow : resource.RecordedAt;
        var result = await commandService.Handle(new RecordTelemetryCommand(resource.SensorId,
            resource.Temperature, resource.Humidity, recordedAt), cancellationToken);
        if (result.IsFailure)
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error?.ToString(),
                detail: result.Message);

        return Created($"/api/v1/shipments/{result.Value!.ShipmentId}/telemetry/{result.Value.Id}",
            TelemetryResourceAssembler.ToResource(result.Value));
    }

    [HttpGet("shipments/{shipmentId:int}/telemetry")]
    [SwaggerOperation(Summary = "List telemetry readings recorded for a shipment")]
    public async Task<IActionResult> GetByShipment(int shipmentId, CancellationToken cancellationToken) =>
        Ok((await queryService.Handle(new GetTelemetryByShipmentIdQuery(shipmentId), cancellationToken))
            .Select(TelemetryResourceAssembler.ToResource));
}
