using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Shared.Application.Model;

namespace FreshGuard.ColdTrack.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<UserAccount>> Handle(SignUpCommand command, CancellationToken cancellationToken);
    Task<Result<(UserAccount User, string Token)>> Handle(SignInCommand command, CancellationToken cancellationToken);
}


