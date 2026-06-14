using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Services;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Acl;

namespace FreshGuard.ColdTrack.Platform.Alerting.Application.Acl;

public class AlertingContextFacade(ThresholdPolicy policy, IAlertRepository repository) : IAlertingContextFacade
{
    public async Task EvaluateTelemetryAsync(int shipmentId, int sensorId, decimal temperature, decimal humidity,
        CancellationToken cancellationToken)
    {
        var sequence = await repository.GetNextCodeSequenceAsync(cancellationToken);
        foreach (var violation in policy.Evaluate(temperature, humidity))
        {
            await repository.AddAsync(new Alert($"ALT-{sequence:000}", shipmentId, sensorId, violation.Type,
                violation.Severity, violation.Message, violation.Value, violation.Limit), cancellationToken);
            sequence++;
        }
    }
}
