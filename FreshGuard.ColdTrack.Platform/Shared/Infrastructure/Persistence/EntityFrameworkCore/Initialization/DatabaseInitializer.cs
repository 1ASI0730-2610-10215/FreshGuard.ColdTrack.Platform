using FreshGuard.ColdTrack.Platform.Iam.Application.Internal.OutboundServices;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Initialization;

public static class DatabaseInitializer
{
    public static async Task SeedDemoDataAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        var context = services.GetRequiredService<AppDbContext>();
        var hashingService = services.GetRequiredService<IHashingService>();

        if (!await context.Set<UserAccount>().AnyAsync(cancellationToken))
        {
            var administrator = new UserAccount("ColdTrack Administrator",
                EmailAddress.Create("admin@coldtrack.local"), hashingService.Hash("Password123!"),
                UserRole.LogisticsAdmin);
            var driver = new UserAccount("ColdTrack Driver", EmailAddress.Create("driver@coldtrack.local"),
                hashingService.Hash("Password123!"), UserRole.Driver);
            await context.AddRangeAsync(administrator, driver);
            await context.SaveChangesAsync(cancellationToken);
        }

        var driverId = await context.Set<UserAccount>()
            .Where(user => user.Role == UserRole.Driver)
            .Select(user => user.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (driverId > 0 && !await context.Set<Shipment>().AnyAsync(cancellationToken))
        {
            await context.AddRangeAsync(
                new Shipment("ENV-001", "Lima", driverId, "Temperature-sensitive food",
                    DateTimeOffset.UtcNow.AddHours(1), DateTimeOffset.UtcNow.AddDays(1)),
                new Shipment("ENV-002", "Arequipa", driverId, "Vaccines",
                    DateTimeOffset.UtcNow.AddHours(2), DateTimeOffset.UtcNow.AddDays(2)));
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Set<Sensor>().AnyAsync(cancellationToken))
        {
            await context.AddRangeAsync(new Sensor("SENS-A123", "ColdTrack CT-100"),
                new Sensor("SENS-B456", "ColdTrack CT-100"));
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
