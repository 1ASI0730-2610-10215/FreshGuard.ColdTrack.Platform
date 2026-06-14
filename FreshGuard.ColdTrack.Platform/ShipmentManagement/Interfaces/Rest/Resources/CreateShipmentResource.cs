using System.ComponentModel.DataAnnotations;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Interfaces.Rest.Resources;

public record CreateShipmentResource(
    [Required, MaxLength(150)] string Destination,
    [Range(1, int.MaxValue)] int DriverId,
    [Required, MaxLength(500)] string CargoDescription,
    DateTimeOffset DepartureDate,
    DateTimeOffset EstimatedArrival);
