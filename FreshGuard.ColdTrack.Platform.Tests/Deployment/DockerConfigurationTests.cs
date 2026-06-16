namespace FreshGuard.ColdTrack.Platform.Tests.Deployment;

public class DockerConfigurationTests
{
    [Fact]
    public void Dockerfile_UsesRenderPortAndProductionRuntime()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Dockerfile");
        var content = File.ReadAllText(path);

        Assert.Contains("mcr.microsoft.com/dotnet/aspnet:10.0", content);
        Assert.Contains("${PORT:-10000}", content);
        Assert.Contains("FreshGuard.ColdTrack.Platform.dll", content);
    }

    [Fact]
    public void DockerIgnore_ExcludesLocalAndGeneratedFolders()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".dockerignore");
        var content = File.ReadAllText(path);

        Assert.Contains(".idea", content);
        Assert.Contains(".ai", content);
        Assert.Contains("**/bin", content);
        Assert.Contains("**/obj", content);
    }
}
