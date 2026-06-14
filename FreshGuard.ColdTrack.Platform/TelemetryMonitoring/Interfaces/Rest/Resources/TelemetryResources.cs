using System.ComponentModel.DataAnnotations;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest.Resources;

public record RegisterSensorResource([Required, MaxLength(30)] string SensorCode,
    [Required, MaxLength(100)] string ModelName);

public record AssignSensorResource([Range(1, int.MaxValue)] int ShipmentId);

public record RecordTelemetryResource([Range(1, int.MaxValue)] int SensorId, decimal Temperature,
    [Range(0, 100)] decimal Humidity, DateTimeOffset RecordedAt);

public record SensorResource(int Id, string SensorCode, string ModelName, string Status, int? ShipmentId,
    DateTimeOffset? AssignedAt, DateTimeOffset? LastReadingAt, decimal? Temperature, decimal? Humidity);

public record TelemetryResource(int Id, int SensorId, int ShipmentId, decimal Temperature, decimal Humidity,
    DateTimeOffset RecordedAt);
