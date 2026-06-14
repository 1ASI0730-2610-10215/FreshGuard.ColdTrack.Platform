using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Repositories;

public interface IShipmentRepository : IBaseRepository<Shipment>
{
    Task<IEnumerable<Shipment>> ListByStatusAsync(ShipmentStatus? status, CancellationToken cancellationToken = default);
    Task<int> GetNextCodeSequenceAsync(CancellationToken cancellationToken = default);
}
