using FreshGuard.ColdTrack.Platform.Shared.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;

/// <summary>Represents a refrigerated shipment and controls its lifecycle.</summary>
public class Shipment : IAuditableEntity
{
    private readonly List<ShipmentStatusHistory> _statusHistory = [];
    private Shipment() { }

    public Shipment(string shipmentCode, string destination, int driverId, string cargoDescription,
        DateTimeOffset departureDate, DateTimeOffset estimatedArrival)
    {
        if (string.IsNullOrWhiteSpace(shipmentCode)) throw new ArgumentException("Shipment code is required.");
        if (string.IsNullOrWhiteSpace(destination)) throw new ArgumentException("Destination is required.");
        if (driverId <= 0) throw new ArgumentOutOfRangeException(nameof(driverId));
        if (string.IsNullOrWhiteSpace(cargoDescription)) throw new ArgumentException("Cargo description is required.");
        if (departureDate >= estimatedArrival)
            throw new ArgumentException("Departure must occur before the estimated arrival.");

        ShipmentCode = shipmentCode;
        Destination = destination.Trim();
        DriverId = driverId;
        CargoDescription = cargoDescription.Trim();
        DepartureDate = departureDate.ToUniversalTime();
        EstimatedArrival = estimatedArrival.ToUniversalTime();
        Status = ShipmentStatus.Registered;
    }

    public int Id { get; private set; }
    public string ShipmentCode { get; private set; } = string.Empty;
    public string Destination { get; private set; } = string.Empty;
    public int DriverId { get; private set; }
    public string CargoDescription { get; private set; } = string.Empty;
    public DateTimeOffset DepartureDate { get; private set; }
    public DateTimeOffset EstimatedArrival { get; private set; }
    public DateTimeOffset? ActualArrival { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public IReadOnlyCollection<ShipmentStatusHistory> StatusHistory => _statusHistory.AsReadOnly();
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void ChangeStatus(ShipmentStatus newStatus, int changedByUserId, string? remarks)
    {
        if (!CanTransitionTo(newStatus))
            throw new InvalidOperationException($"Shipment cannot transition from {Status} to {newStatus}.");

        var previousStatus = Status;
        Status = newStatus;
        if (newStatus == ShipmentStatus.Completed) ActualArrival = DateTimeOffset.UtcNow;
        _statusHistory.Add(new ShipmentStatusHistory(previousStatus, newStatus, changedByUserId, remarks));
    }

    private bool CanTransitionTo(ShipmentStatus target) => Status switch
    {
        ShipmentStatus.Registered => target is ShipmentStatus.InTransit or ShipmentStatus.Cancelled,
        ShipmentStatus.InTransit => target is ShipmentStatus.Completed or ShipmentStatus.Cancelled,
        _ => false
    };
}

public enum ShipmentStatus
{
    Registered,
    InTransit,
    Completed,
    Cancelled
}
