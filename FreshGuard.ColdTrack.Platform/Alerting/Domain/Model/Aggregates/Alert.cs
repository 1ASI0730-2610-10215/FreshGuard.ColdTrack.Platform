using FreshGuard.ColdTrack.Platform.Shared.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;

/// <summary>Represents an environmental incident detected during a shipment.</summary>
public class Alert : IAuditableEntity
{
    private Alert() { }

    public Alert(string alertCode, int shipmentId, int sensorId, AlertType type, AlertSeverity severity,
        string message, decimal value, decimal limit)
    {
        AlertCode = alertCode;
        ShipmentId = shipmentId;
        SensorId = sensorId;
        Type = type;
        Severity = severity;
        Message = message;
        Value = value;
        Limit = limit;
        Status = AlertStatus.Triggered;
        TriggeredAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; private set; }
    public string AlertCode { get; private set; } = string.Empty;
    public int ShipmentId { get; private set; }
    public int SensorId { get; private set; }
    public AlertType Type { get; private set; }
    public AlertSeverity Severity { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public decimal Value { get; private set; }
    public decimal Limit { get; private set; }
    public AlertStatus Status { get; private set; }
    public DateTimeOffset TriggeredAt { get; private set; }
    public DateTimeOffset? AcknowledgedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public int? ResolvedByUserId { get; private set; }
    public string? ResolutionNotes { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Acknowledge()
    {
        if (Status != AlertStatus.Triggered)
            throw new InvalidOperationException("Only triggered alerts can be acknowledged.");
        Status = AlertStatus.Acknowledged;
        AcknowledgedAt = DateTimeOffset.UtcNow;
    }

    public void Resolve(int userId, string? notes)
    {
        if (Status == AlertStatus.Resolved)
            throw new InvalidOperationException("The alert is already resolved.");
        Status = AlertStatus.Resolved;
        ResolvedAt = DateTimeOffset.UtcNow;
        ResolvedByUserId = userId;
        ResolutionNotes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
    }
}

public enum AlertType { LowTemperature, HighTemperature, HighHumidity }
public enum AlertSeverity { Warning, Critical }
public enum AlertStatus { Triggered, Acknowledged, Resolved }
