namespace FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Resources;

public record AuthenticatedUserResource(string Token, UserResource User);
