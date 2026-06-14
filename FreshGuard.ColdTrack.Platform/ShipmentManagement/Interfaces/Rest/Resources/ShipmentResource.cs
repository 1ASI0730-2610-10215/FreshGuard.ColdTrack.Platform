namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Interfaces.Rest.Resources;

public record ShipmentResource(
    int Id,
    string ShipmentCode,
    string Destination,
    int DriverId,
    string CargoDescription,
    DateTimeOffset DepartureDate,
    DateTimeOffset EstimatedArrival,
    DateTimeOffset? ActualArrival,
    string Status);
