using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ShipmentRepository(AppDbContext context) : BaseRepository<Shipment>(context), IShipmentRepository
{
    public async Task<IEnumerable<Shipment>> ListByStatusAsync(ShipmentStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Shipment>().AsNoTracking().AsQueryable();
        if (status.HasValue) query = query.Where(shipment => shipment.Status == status.Value);
        return await query.OrderByDescending(shipment => shipment.CreatedAt).ToListAsync(cancellationToken);
    }

    public async Task<int> GetNextCodeSequenceAsync(CancellationToken cancellationToken = default)
    {
        var codes = await Context.Set<Shipment>().Select(shipment => shipment.ShipmentCode)
            .ToListAsync(cancellationToken);
        var maximum = codes.Select(code => int.TryParse(code.Replace("ENV-", string.Empty), out var value) ? value : 0)
            .DefaultIfEmpty(0).Max();
        return maximum + 1;
    }

    public new Task<Shipment?> FindByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Context.Set<Shipment>().Include(shipment => shipment.StatusHistory)
            .FirstOrDefaultAsync(shipment => shipment.Id == id, cancellationToken);
}
