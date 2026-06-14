using System.ComponentModel.DataAnnotations;

namespace FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Rest.Resources;

public record ResolveAlertResource([MaxLength(500)] string? Notes);

public record AlertResource(int Id, string AlertCode, int ShipmentId, int SensorId, string Type, string Severity,
    string Status, string Message, decimal Value, decimal Limit, DateTimeOffset TriggeredAt,
    DateTimeOffset? AcknowledgedAt, DateTimeOffset? ResolvedAt, int? ResolvedByUserId, string? ResolutionNotes);
