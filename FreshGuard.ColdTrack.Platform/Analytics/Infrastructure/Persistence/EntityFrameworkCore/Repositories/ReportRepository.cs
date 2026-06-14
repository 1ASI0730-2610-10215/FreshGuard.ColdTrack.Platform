using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ReportRepository(AppDbContext context) : BaseRepository<Report>(context), IReportRepository
{
    public async Task<int> GetNextCodeSequenceAsync(CancellationToken cancellationToken = default)
    {
        var codes = await Context.Set<Report>().Select(report => report.ReportCode).ToListAsync(cancellationToken);
        return codes.Select(code => int.TryParse(code.Replace("RPT-", string.Empty), out var value) ? value : 0)
            .DefaultIfEmpty(0).Max() + 1;
    }

    public new async Task<IEnumerable<Report>> ListAsync(CancellationToken cancellationToken = default) =>
        await Context.Set<Report>().AsNoTracking().OrderByDescending(report => report.GeneratedAt)
            .ToListAsync(cancellationToken);
}
