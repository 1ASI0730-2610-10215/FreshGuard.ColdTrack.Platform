using System.ComponentModel.DataAnnotations;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Interfaces.Rest.Resources;

public record UpdateShipmentStatusResource([Required] string Status, [MaxLength(300)] string? Remarks);
