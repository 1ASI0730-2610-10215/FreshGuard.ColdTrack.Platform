using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserAccountRepository(AppDbContext context) : BaseRepository<UserAccount>(context), IUserAccountRepository
{
    public Task<UserAccount?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        var email = EmailAddress.Create(normalizedEmail);
        return Context.Set<UserAccount>().FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        var email = EmailAddress.Create(normalizedEmail);
        return Context.Set<UserAccount>().AnyAsync(user => user.Email == email, cancellationToken);
    }
}
