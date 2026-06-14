using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace FreshGuard.ColdTrack.Platform.Tests.Analytics.Domain;

public class ReportTests
{
    [Fact]
    public void Constructor_WithMetrics_CreatesTraceableSnapshot()
    {
        var period = new DateRange(DateTimeOffset.UtcNow.AddDays(-30), DateTimeOffset.UtcNow);

        var report = new Report("RPT-001", period, 5, 5, 3, 4.2m, 53m, 1);

        Assert.Equal("RPT-001", report.ReportCode);
        Assert.Equal(5, report.CompletedShipments);
        Assert.Equal(3, report.TotalAlerts);
        Assert.Equal(1, report.GeneratedByUserId);
    }
}
