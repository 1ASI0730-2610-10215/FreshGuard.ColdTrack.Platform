using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Queries;

public record GetDashboardQuery;

public record GetShipmentHistoryQuery(DateRange Period);

public record GetReportByIdQuery(int ReportId);

public record GetReportsQuery;
