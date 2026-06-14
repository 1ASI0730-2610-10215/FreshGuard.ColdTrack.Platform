namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Commands;

public record CreateShipmentCommand(string Destination, int DriverId, string CargoDescription,
    DateTimeOffset DepartureDate, DateTimeOffset EstimatedArrival);
