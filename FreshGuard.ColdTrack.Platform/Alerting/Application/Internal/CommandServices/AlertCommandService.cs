using FreshGuard.ColdTrack.Platform.Alerting.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Shared.Application.Model;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.Alerting.Application.Internal.CommandServices;

public class AlertCommandService(IAlertRepository repository, IUnitOfWork unitOfWork) : IAlertCommandService
{
    public Task<Result<Alert>> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken) =>
        Update(command.AlertId, alert => alert.Acknowledge(), cancellationToken);

    public Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken) =>
        Update(command.AlertId, alert => alert.Resolve(command.UserId, command.Notes), cancellationToken);

    private async Task<Result<Alert>> Update(int id, Action<Alert> action, CancellationToken cancellationToken)
    {
        var alert = await repository.FindByIdAsync(id, cancellationToken);
        if (alert is null)
            return Result<Alert>.Failure(AlertingError.AlertNotFound, "The alert was not found.");

        try
        {
            action(alert);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Alert>.Failure(AlertingError.InvalidAlertTransition, exception.Message);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Alert>.Success(alert);
    }
}
