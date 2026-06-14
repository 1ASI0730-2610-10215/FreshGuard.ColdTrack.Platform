using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Resources;

namespace FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResource(UserAccount user) =>
        new(user.Id, user.FullName, user.Email.Value, user.Role.ToString());
}


