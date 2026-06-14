using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.Iam.Domain.Repositories;

public interface IUserAccountRepository : IBaseRepository<UserAccount>
{
    Task<UserAccount?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
}


