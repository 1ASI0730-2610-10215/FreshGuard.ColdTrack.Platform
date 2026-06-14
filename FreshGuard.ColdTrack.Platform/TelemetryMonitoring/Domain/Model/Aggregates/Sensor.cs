using FreshGuard.ColdTrack.Platform.Shared.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;

/// <summary>
///     Represents a physical IoT sensor used to monitor a refrigerated shipment.
/// </summary>
public class Sensor : IAuditableEntity
{
    private Sensor()
    {
    }

    public Sensor(string sensorCode, string modelName)
    {
        if (string.IsNullOrWhiteSpace(sensorCode)) throw new ArgumentException("Sensor code is required.");
        if (string.IsNullOrWhiteSpace(modelName)) throw new ArgumentException("Model name is required.");

        SensorCode = sensorCode.Trim().ToUpperInvariant();
        ModelName = modelName.Trim();
        Status = SensorStatus.Available;
    }

    public int Id { get; private set; }
    public string SensorCode { get; private set; } = string.Empty;
    public string ModelName { get; private set; } = string.Empty;
    public SensorStatus Status { get; private set; }
    public int? ShipmentId { get; private set; }
    public DateTimeOffset? AssignedAt { get; private set; }
    public DateTimeOffset? LastReadingAt { get; private set; }
    public decimal? Temperature { get; private set; }
    public decimal? Humidity { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void AssignToShipment(int shipmentId)
    {
        if (shipmentId <= 0) throw new ArgumentOutOfRangeException(nameof(shipmentId));
        if (Status != SensorStatus.Available) throw new InvalidOperationException("The sensor is not available.");

        ShipmentId = shipmentId;
        AssignedAt = DateTimeOffset.UtcNow;
        Status = SensorStatus.Assigned;
    }

    public void RecordReading(decimal temperature, decimal humidity, DateTimeOffset recordedAt)
    {
        if (Status != SensorStatus.Assigned || ShipmentId is null)
            throw new InvalidOperationException("The sensor must be assigned before recording telemetry.");
        if (humidity is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(humidity));

        Temperature = temperature;
        Humidity = humidity;
        LastReadingAt = recordedAt.ToUniversalTime();
    }
}

public enum SensorStatus
{
    Available,
    Assigned,
    Inactive
}
