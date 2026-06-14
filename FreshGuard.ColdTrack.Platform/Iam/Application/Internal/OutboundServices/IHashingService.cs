namespace FreshGuard.ColdTrack.Platform.Iam.Application.Internal.OutboundServices;

public interface IHashingService
{
    string Hash(string value);
    bool Verify(string value, string hash);
}
