using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Report>(entity =>
        {
            entity.HasKey(report => report.Id);
            entity.Property(report => report.Id).ValueGeneratedOnAdd();
            entity.Property(report => report.ReportCode).IsRequired().HasMaxLength(20);
            entity.HasIndex(report => report.ReportCode).IsUnique();
            entity.Property(report => report.AverageTemperature).HasPrecision(6, 2);
            entity.Property(report => report.AverageHumidity).HasPrecision(5, 2);
            entity.OwnsOne(report => report.Period, period =>
            {
                period.Property(value => value.Start).HasColumnName("period_start");
                period.Property(value => value.End).HasColumnName("period_end");
            });
        });
    }
}
