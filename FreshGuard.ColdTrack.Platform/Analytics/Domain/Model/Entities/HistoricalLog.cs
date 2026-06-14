namespace FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Entities;

/// <summary>Represents a denormalized shipment row optimized for history and report output.</summary>
public record HistoricalLog(int ShipmentId, string ShipmentCode, string Destination, string DriverName,
    string CargoDescription, string Status, DateTimeOffset DepartureDate, DateTimeOffset EstimatedArrival,
    DateTimeOffset? ActualArrival, decimal? AverageTemperature, decimal? AverageHumidity, int AlertCount);

/// <summary>Represents the operational indicators displayed by the ColdTrack dashboard.</summary>
public record DashboardSummary(int TotalShipments, int ActiveShipments, int CompletedShipments,
    int CancelledShipments, int TotalSensors, int AssignedSensors, int ActiveAlerts, int CriticalAlerts);
