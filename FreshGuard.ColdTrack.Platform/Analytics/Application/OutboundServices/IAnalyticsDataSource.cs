using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace FreshGuard.ColdTrack.Platform.Analytics.Application.OutboundServices;

public interface IAnalyticsDataSource
{
    Task<DashboardSummary> GetDashboardAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<HistoricalLog>> GetHistoryAsync(DateRange period, CancellationToken cancellationToken);
}
