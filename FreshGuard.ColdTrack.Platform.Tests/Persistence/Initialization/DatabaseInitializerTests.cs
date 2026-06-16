using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Initialization;

namespace FreshGuard.ColdTrack.Platform.Tests.Persistence.Initialization;

public class DatabaseInitializerTests
{
    [Fact]
    public void DatabaseInitializer_ExposesSeedDemoDataOperation()
    {
        var method = typeof(DatabaseInitializer).GetMethod(nameof(DatabaseInitializer.SeedDemoDataAsync));

        Assert.NotNull(method);
    }
}
