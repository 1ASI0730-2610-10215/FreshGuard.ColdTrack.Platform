using FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.Internal.QueryServices;

public class ShipmentQueryService(IShipmentRepository repository) : IShipmentQueryService
{
    public Task<Shipment?> Handle(GetShipmentByIdQuery query, CancellationToken cancellationToken) =>
        repository.FindByIdAsync(query.ShipmentId, cancellationToken);

    public Task<IEnumerable<Shipment>> Handle(GetAllShipmentsQuery query, CancellationToken cancellationToken) =>
        repository.ListByStatusAsync(query.Status, cancellationToken);
}
