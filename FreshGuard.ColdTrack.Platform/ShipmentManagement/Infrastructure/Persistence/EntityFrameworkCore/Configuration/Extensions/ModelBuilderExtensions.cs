using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyShipmentManagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Shipment>(entity =>
        {
            entity.HasKey(shipment => shipment.Id);
            entity.Property(shipment => shipment.Id).ValueGeneratedOnAdd();
            entity.Property(shipment => shipment.ShipmentCode).IsRequired().HasMaxLength(20);
            entity.HasIndex(shipment => shipment.ShipmentCode).IsUnique();
            entity.Property(shipment => shipment.Destination).IsRequired().HasMaxLength(150);
            entity.Property(shipment => shipment.CargoDescription).IsRequired().HasMaxLength(500);
            entity.Property(shipment => shipment.Status).HasConversion<string>().IsRequired().HasMaxLength(30);
            entity.HasMany(shipment => shipment.StatusHistory)
                .WithOne()
                .HasForeignKey(history => history.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Navigation(shipment => shipment.StatusHistory).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Entity<ShipmentStatusHistory>(entity =>
        {
            entity.HasKey(history => history.Id);
            entity.Property(history => history.PreviousStatus).HasConversion<string>().HasMaxLength(30);
            entity.Property(history => history.NewStatus).HasConversion<string>().HasMaxLength(30);
            entity.Property(history => history.Remarks).HasMaxLength(300);
        });
    }
}
