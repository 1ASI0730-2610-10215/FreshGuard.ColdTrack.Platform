namespace FreshGuard.ColdTrack.Platform.Tests.Configuration;

public class ProductionConfigurationTests
{
    [Fact]
    public void ProductionConfiguration_UsesPlaceholdersInsteadOfSecrets()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..",
            "FreshGuard.ColdTrack.Platform", "appsettings.Production.json");
        var content = File.ReadAllText(path);

        Assert.Contains("%DATABASE_HOST%", content);
        Assert.Contains("%DATABASE_PASSWORD%", content);
        Assert.Contains("SslMode=Required", content);
        Assert.Contains("Allow User Variables=True", content);
        Assert.DoesNotContain(".filess.io", content);
    }
}
