namespace FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;

public class TokenSettings
{
    public const string SectionName = "TokenSettings";
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationHours { get; set; } = 8;
}
