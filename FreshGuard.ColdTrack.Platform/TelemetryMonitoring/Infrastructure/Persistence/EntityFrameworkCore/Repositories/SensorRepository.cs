using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SensorRepository(AppDbContext context) : BaseRepository<Sensor>(context), ISensorRepository
{
    public Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        Context.Set<Sensor>().AnyAsync(sensor => sensor.SensorCode == code, cancellationToken);
}
