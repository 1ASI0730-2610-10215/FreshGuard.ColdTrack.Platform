using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyTelemetryMonitoringConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Sensor>(entity =>
        {
            entity.ToTable("sensors");
            entity.HasKey(sensor => sensor.Id).HasName("p_k_sensors");
            entity.Property(sensor => sensor.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(sensor => sensor.SensorCode).HasColumnName("sensor_code").IsRequired().HasMaxLength(30);
            entity.HasIndex(sensor => sensor.SensorCode).HasDatabaseName("i_x_sensors_sensor_code").IsUnique();
            entity.Property(sensor => sensor.ModelName).HasColumnName("model_name").IsRequired().HasMaxLength(100);
            entity.Property(sensor => sensor.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20);
            entity.Property(sensor => sensor.ShipmentId).HasColumnName("shipment_id");
            entity.Property(sensor => sensor.AssignedAt).HasColumnName("assigned_at");
            entity.Property(sensor => sensor.LastReadingAt).HasColumnName("last_reading_at");
            entity.Property(sensor => sensor.Temperature).HasColumnName("temperature").HasPrecision(6, 2);
            entity.Property(sensor => sensor.Humidity).HasColumnName("humidity").HasPrecision(5, 2);
            entity.Property(sensor => sensor.CreatedAt).HasColumnName("created_at");
            entity.Property(sensor => sensor.UpdatedAt).HasColumnName("updated_at");
        });

        builder.Entity<TelemetryLog>(entity =>
        {
            entity.ToTable("telemetry_logs");
            entity.HasKey(log => log.Id).HasName("p_k_telemetry_logs");
            entity.Property(log => log.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(log => log.SensorId).HasColumnName("sensor_id");
            entity.Property(log => log.ShipmentId).HasColumnName("shipment_id");
            entity.Property(log => log.Temperature).HasColumnName("temperature").HasPrecision(6, 2);
            entity.Property(log => log.Humidity).HasColumnName("humidity").HasPrecision(5, 2);
            entity.Property(log => log.RecordedAt).HasColumnName("recorded_at");
            entity.Property(log => log.CreatedAt).HasColumnName("created_at");
            entity.Property(log => log.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(log => new { log.ShipmentId, log.RecordedAt })
                .HasDatabaseName("i_x_telemetry_logs_shipment_id_recorded_at");
        });
    }
}
