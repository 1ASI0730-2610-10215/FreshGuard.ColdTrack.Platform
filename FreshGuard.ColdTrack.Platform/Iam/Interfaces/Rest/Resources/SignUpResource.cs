using System.ComponentModel.DataAnnotations;

namespace FreshGuard.ColdTrack.Platform.Iam.Interfaces.Rest.Resources;

public record SignUpResource(
    [Required, MaxLength(120)] string FullName,
    [Required, EmailAddress, MaxLength(254)] string Email,
    [Required, MinLength(8)] string Password,
    [Required] string Role);


