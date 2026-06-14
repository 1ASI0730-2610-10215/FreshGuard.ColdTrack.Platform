using FreshGuard.ColdTrack.Platform.Iam.Application.Internal.OutboundServices;

namespace FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;

public class HashingService : IHashingService
{
    public string Hash(string value) => global::BCrypt.Net.BCrypt.HashPassword(value);
    public bool Verify(string value, string hash) => global::BCrypt.Net.BCrypt.Verify(value, hash);
}
