using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

namespace FreshGuard.ColdTrack.Platform.Tests.Integration;

public class AuthenticationTests
{
    [Fact]
    public async Task GetCurrentUser_WithoutToken_ReturnsUnauthorized()
    {
        await using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.UseSetting("Database:InitializeOnStartup", "false");
                builder.UseSetting("ConnectionStrings:DefaultConnection",
                    "server=localhost;port=3306;database=coldtrack_test;user=root;password=password");
                builder.UseSetting("TokenSettings:Secret", "coldtrack-testing-secret-key-with-32-characters");
                builder.UseSetting("TokenSettings:Issuer", "FreshGuard.ColdTrack.Platform.Tests");
                builder.UseSetting("TokenSettings:Audience", "ColdTrack.Tests");
            });
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync("/api/v1/users/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
