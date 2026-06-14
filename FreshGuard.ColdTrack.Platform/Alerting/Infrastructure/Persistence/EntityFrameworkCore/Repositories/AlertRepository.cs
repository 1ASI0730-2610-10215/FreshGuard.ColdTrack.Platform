using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository(AppDbContext context) : BaseRepository<Alert>(context), IAlertRepository
{
    public async Task<IEnumerable<Alert>> ListFilteredAsync(AlertStatus? status, AlertSeverity? severity,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Alert>().AsNoTracking().AsQueryable();
        if (status.HasValue) query = query.Where(alert => alert.Status == status);
        if (severity.HasValue) query = query.Where(alert => alert.Severity == severity);
        return await query.OrderByDescending(alert => alert.TriggeredAt).ToListAsync(cancellationToken);
    }

    public async Task<int> GetNextCodeSequenceAsync(CancellationToken cancellationToken = default)
    {
        var codes = await Context.Set<Alert>().Select(alert => alert.AlertCode).ToListAsync(cancellationToken);
        return codes.Select(code => int.TryParse(code.Replace("ALT-", string.Empty), out var value) ? value : 0)
            .DefaultIfEmpty(0).Max() + 1;
    }
}
