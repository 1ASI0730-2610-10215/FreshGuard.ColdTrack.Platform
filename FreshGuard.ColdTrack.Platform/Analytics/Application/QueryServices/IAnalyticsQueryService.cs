using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Queries;

namespace FreshGuard.ColdTrack.Platform.Analytics.Application.QueryServices;

public interface IAnalyticsQueryService
{
    Task<DashboardSummary> Handle(GetDashboardQuery query, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<HistoricalLog>> Handle(GetShipmentHistoryQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Report>> Handle(GetReportsQuery query, CancellationToken cancellationToken);
    Task<Report?> Handle(GetReportByIdQuery query, CancellationToken cancellationToken);
}
