using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Repositories;

public interface ISensorRepository : IBaseRepository<Sensor>
{
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
