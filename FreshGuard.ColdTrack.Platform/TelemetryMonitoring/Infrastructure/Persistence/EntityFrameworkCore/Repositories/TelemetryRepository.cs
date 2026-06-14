using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TelemetryRepository(AppDbContext context) : BaseRepository<TelemetryLog>(context), ITelemetryRepository
{
    public async Task<IEnumerable<TelemetryLog>> ListByShipmentIdAsync(int shipmentId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<TelemetryLog>()
            .AsNoTracking()
            .Where(log => log.ShipmentId == shipmentId)
            .OrderByDescending(log => log.RecordedAt)
            .ToListAsync(cancellationToken);
}
