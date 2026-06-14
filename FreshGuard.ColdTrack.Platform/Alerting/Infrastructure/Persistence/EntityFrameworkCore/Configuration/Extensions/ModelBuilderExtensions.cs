using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAlertingConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Alert>(entity =>
        {
            entity.HasKey(alert => alert.Id);
            entity.Property(alert => alert.Id).ValueGeneratedOnAdd();
            entity.Property(alert => alert.AlertCode).IsRequired().HasMaxLength(20);
            entity.HasIndex(alert => alert.AlertCode).IsUnique();
            entity.Property(alert => alert.Type).HasConversion<string>().HasMaxLength(30);
            entity.Property(alert => alert.Severity).HasConversion<string>().HasMaxLength(20);
            entity.Property(alert => alert.Status).HasConversion<string>().HasMaxLength(20);
            entity.Property(alert => alert.Message).IsRequired().HasMaxLength(300);
            entity.Property(alert => alert.Value).HasPrecision(6, 2);
            entity.Property(alert => alert.Limit).HasPrecision(6, 2);
            entity.Property(alert => alert.ResolutionNotes).HasMaxLength(500);
            entity.HasIndex(alert => new { alert.ShipmentId, alert.Status });
        });
    }
}
