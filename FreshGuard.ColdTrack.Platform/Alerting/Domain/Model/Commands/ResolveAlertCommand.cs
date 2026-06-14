namespace FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Commands;

public record ResolveAlertCommand(int AlertId, int UserId, string? Notes);
