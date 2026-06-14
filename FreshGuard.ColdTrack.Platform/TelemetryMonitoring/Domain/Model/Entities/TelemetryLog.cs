using FreshGuard.ColdTrack.Platform.Shared.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;

/// <summary>
///     Represents an immutable environmental reading produced by a sensor.
/// </summary>
public class TelemetryLog : IAuditableEntity
{
    private TelemetryLog()
    {
    }

    public TelemetryLog(int sensorId, int shipmentId, decimal temperature, decimal humidity,
        DateTimeOffset recordedAt)
    {
        if (sensorId <= 0) throw new ArgumentOutOfRangeException(nameof(sensorId));
        if (shipmentId <= 0) throw new ArgumentOutOfRangeException(nameof(shipmentId));
        if (humidity is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(humidity));

        SensorId = sensorId;
        ShipmentId = shipmentId;
        Temperature = temperature;
        Humidity = humidity;
        RecordedAt = recordedAt.ToUniversalTime();
    }

    public int Id { get; private set; }
    public int SensorId { get; private set; }
    public int ShipmentId { get; private set; }
    public decimal Temperature { get; private set; }
    public decimal Humidity { get; private set; }
    public DateTimeOffset RecordedAt { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
