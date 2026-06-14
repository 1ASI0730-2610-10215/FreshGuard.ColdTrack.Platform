using FreshGuard.ColdTrack.Platform.Alerting.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.Alerting.Application.Internal.QueryServices;

public class AlertQueryService(IAlertRepository repository) : IAlertQueryService
{
    public Task<IEnumerable<Alert>> Handle(GetAlertsQuery query, CancellationToken cancellationToken) =>
        repository.ListFilteredAsync(query.Status, query.Severity, cancellationToken);
}
