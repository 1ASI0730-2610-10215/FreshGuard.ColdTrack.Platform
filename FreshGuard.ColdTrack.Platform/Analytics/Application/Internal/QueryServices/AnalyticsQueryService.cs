using FreshGuard.ColdTrack.Platform.Analytics.Application.OutboundServices;
using FreshGuard.ColdTrack.Platform.Analytics.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.Analytics.Application.Internal.QueryServices;

public class AnalyticsQueryService(IAnalyticsDataSource dataSource, IReportRepository reportRepository)
    : IAnalyticsQueryService
{
    public Task<DashboardSummary> Handle(GetDashboardQuery query, CancellationToken cancellationToken) =>
        dataSource.GetDashboardAsync(cancellationToken);

    public Task<IReadOnlyCollection<HistoricalLog>> Handle(GetShipmentHistoryQuery query,
        CancellationToken cancellationToken) => dataSource.GetHistoryAsync(query.Period, cancellationToken);

    public Task<IEnumerable<Report>> Handle(GetReportsQuery query, CancellationToken cancellationToken) =>
        reportRepository.ListAsync(cancellationToken);

    public Task<Report?> Handle(GetReportByIdQuery query, CancellationToken cancellationToken) =>
        reportRepository.FindByIdAsync(query.ReportId, cancellationToken);
}
