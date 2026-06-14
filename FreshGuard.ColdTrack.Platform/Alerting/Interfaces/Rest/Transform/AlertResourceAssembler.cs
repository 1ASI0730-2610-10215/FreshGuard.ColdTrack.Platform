using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Rest.Resources;

namespace FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Rest.Transform;

public static class AlertResourceAssembler
{
    public static AlertResource ToResource(Alert alert) => new(alert.Id, alert.AlertCode, alert.ShipmentId,
        alert.SensorId, alert.Type.ToString(), alert.Severity.ToString(), alert.Status.ToString(), alert.Message,
        alert.Value, alert.Limit, alert.TriggeredAt, alert.AcknowledgedAt, alert.ResolvedAt,
        alert.ResolvedByUserId, alert.ResolutionNotes);
}
