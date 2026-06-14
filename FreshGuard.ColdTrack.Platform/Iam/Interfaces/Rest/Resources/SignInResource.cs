using System.ComponentModel.DataAnnotations;

namespace FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Resources;

public record SignInResource([Required, EmailAddress] string Email, [Required] string Password);
