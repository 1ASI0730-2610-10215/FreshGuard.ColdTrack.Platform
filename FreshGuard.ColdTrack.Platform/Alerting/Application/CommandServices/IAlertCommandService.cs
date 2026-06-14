using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Shared.Application.Model;

namespace FreshGuard.ColdTrack.Platform.Alerting.Application.CommandServices;

public interface IAlertCommandService
{
    Task<Result<Alert>> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken);
    Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken);
}
