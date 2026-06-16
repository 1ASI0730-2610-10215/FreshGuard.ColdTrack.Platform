namespace FreshGuard.ColdTrack.Platform.Tests.Persistence.Migrations;

public class CoreSchemaMigrationTests
{
    [Fact]
    public void RepairCoreSchemaMigration_ContainsMissingCoreTables()
    {
        const string migration = "FreshGuard.ColdTrack.Platform.Migrations.RepairCoreSchema";
        var type = typeof(Program).Assembly.GetType(migration);

        Assert.NotNull(type);

        var file = File.ReadAllText(Path.Combine(AppContext.BaseDirectory,
            "..", "..", "..", "..", "FreshGuard.ColdTrack.Platform", "Migrations",
            "20260616050000_RepairCoreSchema.cs"));

        Assert.Contains("CREATE TABLE IF NOT EXISTS `UserAccount`", file);
        Assert.Contains("CREATE TABLE IF NOT EXISTS `Shipment`", file);
        Assert.Contains("CREATE TABLE IF NOT EXISTS `ShipmentStatusHistory`", file);
    }
}
