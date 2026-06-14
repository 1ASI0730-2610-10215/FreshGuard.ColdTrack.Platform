using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Queries;

public record GetAlertsQuery(AlertStatus? Status, AlertSeverity? Severity);
