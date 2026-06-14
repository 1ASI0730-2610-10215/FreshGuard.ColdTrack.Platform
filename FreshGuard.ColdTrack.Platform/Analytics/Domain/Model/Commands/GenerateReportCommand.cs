using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Commands;

public record GenerateReportCommand(DateRange Period, int GeneratedByUserId);
