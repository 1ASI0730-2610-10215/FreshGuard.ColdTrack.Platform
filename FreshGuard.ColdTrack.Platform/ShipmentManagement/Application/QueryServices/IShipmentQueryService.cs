using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Queries;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.QueryServices;

public interface IShipmentQueryService
{
    Task<Shipment?> Handle(GetShipmentByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Shipment>> Handle(GetAllShipmentsQuery query, CancellationToken cancellationToken);
}
