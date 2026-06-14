using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Application.OutboundServices;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Queries;

public class AnalyticsDataSource(AppDbContext context) : IAnalyticsDataSource
{
    public async Task<DashboardSummary> GetDashboardAsync(CancellationToken cancellationToken)
    {
        var shipments = context.Set<Shipment>().AsNoTracking();
        var sensors = context.Set<Sensor>().AsNoTracking();
        var alerts = context.Set<Alert>().AsNoTracking();
        return new DashboardSummary(await shipments.CountAsync(cancellationToken),
            await shipments.CountAsync(value => value.Status == ShipmentStatus.Registered ||
                                                  value.Status == ShipmentStatus.InTransit, cancellationToken),
            await shipments.CountAsync(value => value.Status == ShipmentStatus.Completed, cancellationToken),
            await shipments.CountAsync(value => value.Status == ShipmentStatus.Cancelled, cancellationToken),
            await sensors.CountAsync(cancellationToken),
            await sensors.CountAsync(value => value.Status == SensorStatus.Assigned, cancellationToken),
            await alerts.CountAsync(value => value.Status != AlertStatus.Resolved, cancellationToken),
            await alerts.CountAsync(value => value.Status != AlertStatus.Resolved &&
                                               value.Severity == AlertSeverity.Critical, cancellationToken));
    }

    public async Task<IReadOnlyCollection<HistoricalLog>> GetHistoryAsync(DateRange period,
        CancellationToken cancellationToken)
    {
        var shipments = await context.Set<Shipment>().AsNoTracking()
            .Where(value => value.Status == ShipmentStatus.Completed && value.DepartureDate >= period.Start &&
                            value.DepartureDate <= period.End)
            .OrderByDescending(value => value.ActualArrival).ToListAsync(cancellationToken);
        if (shipments.Count == 0) return [];

        var shipmentIds = shipments.Select(value => value.Id).ToArray();
        var driverIds = shipments.Select(value => value.DriverId).Distinct().ToArray();
        var drivers = await context.Set<UserAccount>().AsNoTracking().Where(value => driverIds.Contains(value.Id))
            .ToDictionaryAsync(value => value.Id, value => value.FullName, cancellationToken);
        var telemetry = await context.Set<TelemetryLog>().AsNoTracking()
            .Where(value => shipmentIds.Contains(value.ShipmentId)).ToListAsync(cancellationToken);
        var alerts = await context.Set<Alert>().AsNoTracking()
            .Where(value => shipmentIds.Contains(value.ShipmentId)).ToListAsync(cancellationToken);

        return shipments.Select(shipment =>
        {
            var readings = telemetry.Where(value => value.ShipmentId == shipment.Id).ToArray();
            return new HistoricalLog(shipment.Id, shipment.ShipmentCode, shipment.Destination,
                drivers.GetValueOrDefault(shipment.DriverId, "Unknown driver"), shipment.CargoDescription,
                shipment.Status.ToString(), shipment.DepartureDate, shipment.EstimatedArrival,
                shipment.ActualArrival,
                readings.Length == 0 ? null : Math.Round(readings.Average(x => x.Temperature), 2),
                readings.Length == 0 ? null : Math.Round(readings.Average(x => x.Humidity), 2),
                alerts.Count(value => value.ShipmentId == shipment.Id));
        }).ToArray();
    }
}
