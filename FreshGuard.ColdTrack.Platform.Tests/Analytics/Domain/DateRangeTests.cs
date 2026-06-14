using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace FreshGuard.ColdTrack.Platform.Tests.Analytics.Domain;

public class DateRangeTests
{
    [Fact]
    public void Constructor_WithValidDates_NormalizesToUtc()
    {
        var start = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.FromHours(-5));
        var end = start.AddDays(1);

        var range = new DateRange(start, end);

        Assert.Equal(TimeSpan.Zero, range.Start.Offset);
        Assert.Equal(TimeSpan.Zero, range.End.Offset);
    }

    [Fact]
    public void Constructor_WithReversedDates_RejectsRange() =>
        Assert.Throws<ArgumentException>(() =>
            new DateRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(-1)));
}
