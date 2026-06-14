using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Repositories;

public interface ITelemetryRepository : IBaseRepository<TelemetryLog>
{
    Task<IEnumerable<TelemetryLog>> ListByShipmentIdAsync(int shipmentId,
        CancellationToken cancellationToken = default);
}
