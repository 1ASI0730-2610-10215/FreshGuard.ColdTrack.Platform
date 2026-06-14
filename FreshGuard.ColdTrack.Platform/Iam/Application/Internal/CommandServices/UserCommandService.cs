using FreshGuard.ColdTrack.Platform.Iam.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Iam.Application.Internal.OutboundServices;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Iam.Resources;
using FreshGuard.ColdTrack.Platform.Shared.Application.Model;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;
using Microsoft.Extensions.Localization;

namespace FreshGuard.ColdTrack.Platform.Iam.Application.Internal.CommandServices;

public class UserCommandService(
    IUserAccountRepository repository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<IamMessages> localizer) : IUserCommandService
{
    public async Task<Result<UserAccount>> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        EmailAddress email;
        try { email = EmailAddress.Create(command.Email); }
        catch (ArgumentException exception) { return Result<UserAccount>.Failure(IamError.InvalidCredentials, exception.Message); }

        if (!Enum.TryParse<UserRole>(command.Role.Replace("_", string.Empty), true, out var role))
            return Result<UserAccount>.Failure(IamError.InvalidRole, localizer["InvalidRole"]);
        if (await repository.ExistsByEmailAsync(email.Value, cancellationToken))
            return Result<UserAccount>.Failure(IamError.EmailAlreadyRegistered, localizer["EmailAlreadyRegistered"]);
        if (command.Password.Length < 8)
            return Result<UserAccount>.Failure(IamError.InvalidCredentials, localizer["PasswordTooShort"]);

        var user = new UserAccount(command.FullName, email, hashingService.Hash(command.Password), role);
        await repository.AddAsync(user, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<UserAccount>.Success(user);
    }

    public async Task<Result<(UserAccount User, string Token)>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var normalizedEmail = command.Email.Trim().ToLowerInvariant();
        var user = await repository.FindByEmailAsync(normalizedEmail, cancellationToken);
        if (user is null || !user.IsActive || !hashingService.Verify(command.Password, user.PasswordHash))
            return Result<(UserAccount, string)>.Failure(IamError.InvalidCredentials, localizer["InvalidCredentials"]);

        return Result<(UserAccount, string)>.Success((user, tokenService.Generate(user)));
    }
}
