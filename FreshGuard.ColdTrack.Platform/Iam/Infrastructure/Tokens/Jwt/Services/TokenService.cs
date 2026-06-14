using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FreshGuard.ColdTrack.Platform.Iam.Application.Internal.OutboundServices;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Services;

public class TokenService(IOptions<TokenSettings> options) : ITokenService
{
    private readonly TokenSettings _settings = options.Value;

    public string Generate(UserAccount user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_settings.Issuer, _settings.Audience, claims,
            expires: DateTime.UtcNow.AddHours(_settings.ExpirationHours), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


