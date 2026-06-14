namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Commands;

public record UpdateShipmentStatusCommand(int ShipmentId, string Status, int ChangedByUserId, string? Remarks);
