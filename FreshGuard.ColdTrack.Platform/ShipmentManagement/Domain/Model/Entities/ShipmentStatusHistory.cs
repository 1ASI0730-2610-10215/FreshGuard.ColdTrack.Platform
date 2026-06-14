using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Entities;

public class ShipmentStatusHistory
{
    private ShipmentStatusHistory() { }

    public ShipmentStatusHistory(ShipmentStatus previousStatus, ShipmentStatus newStatus, int changedByUserId, string? remarks)
    {
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        ChangedByUserId = changedByUserId;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        ChangedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; private set; }
    public int ShipmentId { get; private set; }
    public ShipmentStatus PreviousStatus { get; private set; }
    public ShipmentStatus NewStatus { get; private set; }
    public int ChangedByUserId { get; private set; }
    public string? Remarks { get; private set; }
    public DateTimeOffset ChangedAt { get; private set; }
}
