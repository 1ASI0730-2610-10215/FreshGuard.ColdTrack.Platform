namespace FreshGuard.ColdTrack.Platform.Analytics.Interfaces.Rest.Resources;

public record GenerateReportResource(DateTimeOffset Start, DateTimeOffset End);

public record DashboardResource(int TotalShipments, int ActiveShipments, int CompletedShipments,
    int CancelledShipments, int TotalSensors, int AssignedSensors, int ActiveAlerts, int CriticalAlerts);

public record HistoricalShipmentResource(int ShipmentId, string ShipmentCode, string Destination, string DriverName,
    string CargoDescription, string Status, DateTimeOffset DepartureDate, DateTimeOffset EstimatedArrival,
    DateTimeOffset? ActualArrival, decimal? AverageTemperature, decimal? AverageHumidity, int AlertCount);

public record ReportResource(int Id, string ReportCode, DateTimeOffset PeriodStart, DateTimeOffset PeriodEnd,
    int TotalShipments, int CompletedShipments, int TotalAlerts, decimal? AverageTemperature,
    decimal? AverageHumidity, int GeneratedByUserId, DateTimeOffset GeneratedAt);
