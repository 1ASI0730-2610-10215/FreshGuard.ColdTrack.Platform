using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.Tests.ShipmentManagement.Domain;

public class ShipmentTests
{
    [Fact]
    public void Constructor_WithInvalidDates_RejectsShipment()
    {
        var departure = DateTimeOffset.UtcNow.AddDays(1);
        Assert.Throws<ArgumentException>(() => new Shipment("ENV-001", "Lima", 1, "Vaccines",
            departure, departure.AddHours(-1)));
    }

    [Fact]
    public void ChangeStatus_WithValidLifecycle_RecordsHistory()
    {
        var shipment = CreateShipment();
        shipment.ChangeStatus(ShipmentStatus.InTransit, 1, "Departure confirmed");
        shipment.ChangeStatus(ShipmentStatus.Completed, 1, "Delivery confirmed");

        Assert.Equal(ShipmentStatus.Completed, shipment.Status);
        Assert.NotNull(shipment.ActualArrival);
        Assert.Equal(2, shipment.StatusHistory.Count);
    }

    [Fact]
    public void ChangeStatus_FromRegisteredToCompleted_RejectsTransition()
    {
        var shipment = CreateShipment();
        Assert.Throws<InvalidOperationException>(() =>
            shipment.ChangeStatus(ShipmentStatus.Completed, 1, null));
    }

    private static Shipment CreateShipment() => new("ENV-001", "Lima", 1, "Vaccines",
        DateTimeOffset.UtcNow.AddHours(1), DateTimeOffset.UtcNow.AddDays(1));
}
