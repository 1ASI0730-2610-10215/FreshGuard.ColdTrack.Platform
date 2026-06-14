namespace FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Acl;

public interface IAlertingContextFacade
{
    Task EvaluateTelemetryAsync(int shipmentId, int sensorId, decimal temperature, decimal humidity,
        CancellationToken cancellationToken);
}
