using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;

/// <summary>Stores a traceable snapshot of cold-chain performance for a requested date range.</summary>
public class Report : IAuditableEntity
{
    private Report()
    {
    }

    public Report(string reportCode, DateRange period, int totalShipments, int completedShipments,
        int totalAlerts, decimal? averageTemperature, decimal? averageHumidity, int generatedByUserId)
    {
        if (string.IsNullOrWhiteSpace(reportCode)) throw new ArgumentException("Report code is required.");
        if (generatedByUserId <= 0) throw new ArgumentOutOfRangeException(nameof(generatedByUserId));
        ReportCode = reportCode.Trim().ToUpperInvariant();
        Period = period;
        TotalShipments = totalShipments;
        CompletedShipments = completedShipments;
        TotalAlerts = totalAlerts;
        AverageTemperature = averageTemperature;
        AverageHumidity = averageHumidity;
        GeneratedByUserId = generatedByUserId;
        GeneratedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; private set; }
    public string ReportCode { get; private set; } = string.Empty;
    public DateRange Period { get; private set; } = null!;
    public int TotalShipments { get; private set; }
    public int CompletedShipments { get; private set; }
    public int TotalAlerts { get; private set; }
    public decimal? AverageTemperature { get; private set; }
    public decimal? AverageHumidity { get; private set; }
    public int GeneratedByUserId { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
