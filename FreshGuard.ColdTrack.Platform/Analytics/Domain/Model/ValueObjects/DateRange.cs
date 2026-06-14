namespace FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;

/// <summary>Represents an inclusive UTC date range used by analytical queries.</summary>
public class DateRange
{
    private DateRange()
    {
    }

    public DateRange(DateTimeOffset start, DateTimeOffset end)
    {
        if (start > end) throw new ArgumentException("The start date cannot be after the end date.");
        Start = start.ToUniversalTime();
        End = end.ToUniversalTime();
    }

    public DateTimeOffset Start { get; private set; }
    public DateTimeOffset End { get; private set; }
}
