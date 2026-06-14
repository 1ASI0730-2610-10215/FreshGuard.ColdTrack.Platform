using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest.Resources;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Interfaces.Rest.Transform;

public static class TelemetryResourceAssembler
{
    public static SensorResource ToResource(Sensor sensor) =>
        new(sensor.Id, sensor.SensorCode, sensor.ModelName, sensor.Status.ToString(), sensor.ShipmentId,
            sensor.AssignedAt, sensor.LastReadingAt, sensor.Temperature, sensor.Humidity);

    public static TelemetryResource ToResource(TelemetryLog log) =>
        new(log.Id, log.SensorId, log.ShipmentId, log.Temperature, log.Humidity, log.RecordedAt);
}
