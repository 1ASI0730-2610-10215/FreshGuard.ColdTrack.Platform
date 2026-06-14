using System.Text;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Analytics.Infrastructure.Documents.QuestPdf;
using QuestPDF.Infrastructure;

namespace FreshGuard.ColdTrack.Platform.Tests.Analytics.Infrastructure;

public class PdfReportGeneratorTests
{
    [Fact]
    public void Generate_WithHistoricalRows_ReturnsPdfDocument()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var period = new DateRange(DateTimeOffset.UtcNow.AddDays(-30), DateTimeOffset.UtcNow);
        var report = new Report("RPT-001", period, 1, 1, 0, 4.5m, 50m, 1);
        HistoricalLog[] history =
        [
            new(1, "ENV-001", "Lima", "ColdTrack Driver", "Vaccines", "Completed",
                period.Start, period.End, period.End, 4.5m, 50m, 0)
        ];

        var content = new PdfReportGenerator().Generate(report, history);

        Assert.True(content.Length > 100);
        Assert.Equal("%PDF", Encoding.ASCII.GetString(content, 0, 4));
    }
}
