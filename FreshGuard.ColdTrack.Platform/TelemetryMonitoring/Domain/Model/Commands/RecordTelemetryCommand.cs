namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Commands;

public record RecordTelemetryCommand(int SensorId, decimal Temperature, decimal Humidity, DateTimeOffset RecordedAt);
