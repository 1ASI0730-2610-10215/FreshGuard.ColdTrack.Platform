using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Interfaces.Rest.Resources;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Interfaces.Rest.Transform;

public static class ShipmentResourceFromEntityAssembler
{
    public static ShipmentResource ToResource(Shipment shipment) => new(
        shipment.Id, shipment.ShipmentCode, shipment.Destination, shipment.DriverId,
        shipment.CargoDescription, shipment.DepartureDate, shipment.EstimatedArrival,
        shipment.ActualArrival, shipment.Status.ToString());
}
