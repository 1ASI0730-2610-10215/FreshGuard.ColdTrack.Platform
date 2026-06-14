using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.Analytics.Domain.Repositories;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<int> GetNextCodeSequenceAsync(CancellationToken cancellationToken = default);
}
