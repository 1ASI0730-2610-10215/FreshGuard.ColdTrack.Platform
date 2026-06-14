using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.Tests.TelemetryMonitoring.Domain;

public class SensorTests
{
    [Fact]
    public void AssignToShipment_WhenAvailable_AssignsSensor()
    {
        var sensor = new Sensor("SENS-001", "CT-100");

        sensor.AssignToShipment(8);

        Assert.Equal(SensorStatus.Assigned, sensor.Status);
        Assert.Equal(8, sensor.ShipmentId);
    }

    [Fact]
    public void RecordReading_WhenAssigned_UpdatesLatestValues()
    {
        var sensor = new Sensor("SENS-001", "CT-100");
        sensor.AssignToShipment(8);
        var recordedAt = DateTimeOffset.UtcNow;

        sensor.RecordReading(4.5m, 55m, recordedAt);

        Assert.Equal(4.5m, sensor.Temperature);
        Assert.Equal(55m, sensor.Humidity);
        Assert.Equal(recordedAt, sensor.LastReadingAt);
    }

    [Fact]
    public void RecordReading_WithInvalidHumidity_RejectsReading()
    {
        var sensor = new Sensor("SENS-001", "CT-100");
        sensor.AssignToShipment(8);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            sensor.RecordReading(4m, 101m, DateTimeOffset.UtcNow));
    }
}
