using System.Net.Mime;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest.Resources;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class SensorsController(ITelemetryCommandService commandService, ITelemetryQueryService queryService)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List registered sensors")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok((await queryService.Handle(new GetAllSensorsQuery(), cancellationToken))
            .Select(TelemetryResourceAssembler.ToResource));

    [HttpPost]
    [Authorize(Roles = "LogisticsAdmin")]
    [SwaggerOperation(Summary = "Register a monitoring sensor")]
    public async Task<IActionResult> Register(RegisterSensorResource resource, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new RegisterSensorCommand(resource.SensorCode, resource.ModelName),
            cancellationToken);
        if (result.IsFailure)
            return Problem(statusCode: StatusCodes.Status409Conflict, title: result.Error?.ToString(),
                detail: result.Message);

        return Created($"/api/v1/sensors/{result.Value!.Id}",
            TelemetryResourceAssembler.ToResource(result.Value));
    }

    [HttpPatch("{sensorId:int}/assignment")]
    [Authorize(Roles = "LogisticsAdmin")]
    [SwaggerOperation(Summary = "Assign an available sensor to a shipment")]
    public async Task<IActionResult> Assign(int sensorId, AssignSensorResource resource,
        CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new AssignSensorCommand(sensorId, resource.ShipmentId),
            cancellationToken);
        if (result.IsSuccess) return Ok(TelemetryResourceAssembler.ToResource(result.Value!));

        var status = result.Error is TelemetryError.SensorNotFound or TelemetryError.ShipmentNotFound
            ? StatusCodes.Status404NotFound
            : StatusCodes.Status409Conflict;
        return Problem(statusCode: status, title: result.Error?.ToString(), detail: result.Message);
    }
}
