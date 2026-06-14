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
    public async Task<Result<Alert>> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = await repository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null)
            return Result<Alert>.Failure(AlertingError.AlertNotFound, "The alert was not found.");

        try
        {
            alert.Acknowledge();
        }
        catch (InvalidOperationException exception)
        {
            return Result<Alert>.Failure(AlertingError.InvalidAlertTransition, exception.Message);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Alert>.Success(alert);
    }
}
