using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.Analytics.Interfaces.Rest.Resources;

namespace FreshGuard.ColdTrack.Platform.Analytics.Interfaces.Rest.Transform;

public static class AnalyticsResourceAssembler
{
    public static DashboardResource ToResource(DashboardSummary value) => new(value.TotalShipments,
        value.ActiveShipments, value.CompletedShipments, value.CancelledShipments, value.TotalSensors,
        value.AssignedSensors, value.ActiveAlerts, value.CriticalAlerts);

    public static HistoricalShipmentResource ToResource(HistoricalLog value) => new(value.ShipmentId,
        value.ShipmentCode, value.Destination, value.DriverName, value.CargoDescription, value.Status,
        value.DepartureDate, value.EstimatedArrival, value.ActualArrival, value.AverageTemperature,
        value.AverageHumidity, value.AlertCount);

    public static ReportResource ToResource(Report value) => new(value.Id, value.ReportCode, value.Period.Start,
        value.Period.End, value.TotalShipments, value.CompletedShipments, value.TotalAlerts,
        value.AverageTemperature, value.AverageHumidity, value.GeneratedByUserId, value.GeneratedAt);
}
