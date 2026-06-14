using FreshGuard.ColdTrack.Platform.Analytics.Application.OutboundServices;
using FreshGuard.ColdTrack.Platform.Analytics.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Shared.Application.Model;

namespace FreshGuard.ColdTrack.Platform.Analytics.Application.Internal.QueryServices;

public class ReportFileService(IReportRepository repository, IAnalyticsDataSource dataSource,
    IPdfReportGenerator generator) : IReportFileService
{
    public async Task<Result<ReportFile>> GenerateAsync(int reportId, CancellationToken cancellationToken)
    {
        var report = await repository.FindByIdAsync(reportId, cancellationToken);
        if (report is null)
            return Result<ReportFile>.Failure(AnalyticsError.ReportNotFound, "The report was not found.");
        var history = await dataSource.GetHistoryAsync(report.Period, cancellationToken);
        return Result<ReportFile>.Success(new ReportFile($"{report.ReportCode}.pdf",
            generator.Generate(report, history)));
    }
}
