using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.ValueObjects;

namespace FreshGuard.ColdTrack.Platform.Tests.Iam.Domain;

public class EmailAddressTests
{
    [Fact]
    public void Create_WithValidEmail_NormalizesAddress()
    {
        var email = EmailAddress.Create("  USER@ColdTrack.com ");
        Assert.Equal("user@coldtrack.com", email.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    public void Create_WithInvalidEmail_ThrowsArgumentException(string value)
    {
        Assert.Throws<ArgumentException>(() => EmailAddress.Create(value));
    }
}
