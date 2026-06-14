using System.Net.Mime;
using FreshGuard.ColdTrack.Platform.Analytics.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Analytics.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FreshGuard.ColdTrack.Platform.Analytics.Interfaces.Rest;

[ApiController, Authorize(Roles = "LogisticsAdmin"), Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AnalyticsController(IAnalyticsQueryService queryService) : ControllerBase
{
    [HttpGet("dashboard"), SwaggerOperation(Summary = "Get consolidated operational dashboard indicators")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken) =>
        Ok(AnalyticsResourceAssembler.ToResource(
            await queryService.Handle(new GetDashboardQuery(), cancellationToken)));

    [HttpGet("shipment-history"), SwaggerOperation(Summary = "Get completed shipment history for a date range")]
    public async Task<IActionResult> GetHistory([FromQuery] DateTimeOffset? from, [FromQuery] DateTimeOffset? to,
        CancellationToken cancellationToken)
    {
        var period = CreatePeriod(from, to);
        if (period is null) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "InvalidDateRange");
        var history = await queryService.Handle(new GetShipmentHistoryQuery(period), cancellationToken);
        return Ok(history.Select(AnalyticsResourceAssembler.ToResource));
    }

    private static DateRange? CreatePeriod(DateTimeOffset? from, DateTimeOffset? to)
    {
        try
        {
            return new DateRange(from ?? DateTimeOffset.UtcNow.AddYears(-1), to ?? DateTimeOffset.UtcNow);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
}
