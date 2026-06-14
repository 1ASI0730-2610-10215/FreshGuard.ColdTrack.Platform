using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.Alerting.Domain.Repositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    Task<IEnumerable<Alert>> ListFilteredAsync(AlertStatus? status, AlertSeverity? severity,
        CancellationToken cancellationToken = default);

    Task<int> GetNextCodeSequenceAsync(CancellationToken cancellationToken = default);
}
