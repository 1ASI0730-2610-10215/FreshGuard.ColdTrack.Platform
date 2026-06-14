using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using FreshGuard.ColdTrack.Platform.Analytics.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Analytics.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Analytics.Interfaces.Rest.Resources;
using FreshGuard.ColdTrack.Platform.Analytics.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FreshGuard.ColdTrack.Platform.Analytics.Interfaces.Rest;

[ApiController, Authorize(Roles = "LogisticsAdmin"), Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReportsController(IReportCommandService commandService, IAnalyticsQueryService queryService,
    IReportFileService fileService) : ControllerBase
{
    [HttpGet, SwaggerOperation(Summary = "List generated analytical reports")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok((await queryService.Handle(new GetReportsQuery(), cancellationToken))
            .Select(AnalyticsResourceAssembler.ToResource));

    [HttpPost, SwaggerOperation(Summary = "Generate and register a cold-chain performance report")]
    public async Task<IActionResult> Generate(GenerateReportResource resource, CancellationToken cancellationToken)
    {
        DateRange period;
        try
        {
            period = new DateRange(resource.Start, resource.End);
        }
        catch (ArgumentException exception)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "InvalidDateRange",
                detail: exception.Message);
        }

        var subject = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(subject, out var userId)) return Unauthorized();
        var result = await commandService.Handle(new GenerateReportCommand(period, userId), cancellationToken);
        return CreatedAtAction(nameof(Download), new { reportId = result.Value!.Id },
            AnalyticsResourceAssembler.ToResource(result.Value));
    }

    [HttpGet("{reportId:int}/file"), Produces(MediaTypeNames.Application.Pdf)]
    [SwaggerOperation(Summary = "Download a generated report as PDF")]
    public async Task<IActionResult> Download(int reportId, CancellationToken cancellationToken)
    {
        var result = await fileService.GenerateAsync(reportId, cancellationToken);
        return result.IsFailure
            ? Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error?.ToString(), detail: result.Message)
            : File(result.Value!.Content, MediaTypeNames.Application.Pdf, result.Value.FileName);
    }
}
