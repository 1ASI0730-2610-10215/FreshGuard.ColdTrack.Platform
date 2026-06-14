using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.Iam.Application.Internal.OutboundServices;

public interface ITokenService
{
    string Generate(UserAccount user);
}


