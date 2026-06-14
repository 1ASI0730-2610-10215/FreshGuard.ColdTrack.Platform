namespace FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Commands;

public record SignUpCommand(string FullName, string Email, string Password, string Role);


