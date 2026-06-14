using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Queries;

namespace FreshGuard.ColdTrack.Platform.Alerting.Application.QueryServices;

public interface IAlertQueryService
{
    Task<IEnumerable<Alert>> Handle(GetAlertsQuery query, CancellationToken cancellationToken);
}
