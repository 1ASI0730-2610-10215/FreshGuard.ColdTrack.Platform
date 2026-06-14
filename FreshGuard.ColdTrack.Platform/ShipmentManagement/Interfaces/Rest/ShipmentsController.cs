using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Interfaces.Rest.Resources;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Interfaces.Rest;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ShipmentsController(IShipmentCommandService commandService, IShipmentQueryService queryService)
    : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "LogisticsAdmin")]
    [SwaggerOperation(Summary = "Register a refrigerated shipment")]
    [ProducesResponseType(typeof(ShipmentResource), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateShipmentResource resource, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new CreateShipmentCommand(resource.Destination, resource.DriverId,
            resource.CargoDescription, resource.DepartureDate, resource.EstimatedArrival), cancellationToken);
        if (result.IsFailure)
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error?.ToString(), detail: result.Message);
        var shipment = result.Value!;
        return CreatedAtAction(nameof(GetById), new { shipmentId = shipment.Id },
            ShipmentResourceFromEntityAssembler.ToResource(shipment));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "List shipments with an optional status filter")]
    public async Task<IActionResult> GetAll([FromQuery] string? status, CancellationToken cancellationToken)
    {
        ShipmentStatus? parsedStatus = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<ShipmentStatus>(status.Replace("_", string.Empty), true, out var value))
                return Problem(statusCode: StatusCodes.Status400BadRequest, title: "InvalidShipmentStatus");
            parsedStatus = value;
        }
        var shipments = await queryService.Handle(new GetAllShipmentsQuery(parsedStatus), cancellationToken);
        return Ok(shipments.Select(ShipmentResourceFromEntityAssembler.ToResource));
    }

    [HttpGet("{shipmentId:int}")]
    [SwaggerOperation(Summary = "Get a shipment by its technical identifier")]
    public async Task<IActionResult> GetById(int shipmentId, CancellationToken cancellationToken)
    {
        var shipment = await queryService.Handle(new GetShipmentByIdQuery(shipmentId), cancellationToken);
        return shipment is null ? NotFound() : Ok(ShipmentResourceFromEntityAssembler.ToResource(shipment));
    }

    [HttpPatch("{shipmentId:int}/status")]
    [Authorize(Roles = "LogisticsAdmin")]
    [SwaggerOperation(Summary = "Update the lifecycle status of a shipment")]
    public async Task<IActionResult> UpdateStatus(int shipmentId, UpdateShipmentStatusResource resource,
        CancellationToken cancellationToken)
    {
        var subject = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(subject, out var userId)) return Unauthorized();
        var result = await commandService.Handle(
            new UpdateShipmentStatusCommand(shipmentId, resource.Status, userId, resource.Remarks), cancellationToken);
        if (result.IsSuccess) return Ok(ShipmentResourceFromEntityAssembler.ToResource(result.Value!));
        var statusCode = result.Error switch
        {
            ShipmentError.ShipmentNotFound => StatusCodes.Status404NotFound,
            ShipmentError.InvalidStatusTransition => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };
        return Problem(statusCode: statusCode, title: result.Error?.ToString(), detail: result.Message);
    }
}
