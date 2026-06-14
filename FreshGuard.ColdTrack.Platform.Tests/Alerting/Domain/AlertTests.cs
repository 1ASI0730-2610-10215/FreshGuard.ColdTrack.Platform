using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.Tests.Alerting.Domain;

public class AlertTests
{
    [Fact]
    public void Acknowledge_ThenResolve_CompletesLifecycle()
    {
        var alert = CreateAlert();

        alert.Acknowledge();
        alert.Resolve(1, "Temperature stabilized");

        Assert.Equal(AlertStatus.Resolved, alert.Status);
        Assert.NotNull(alert.AcknowledgedAt);
        Assert.NotNull(alert.ResolvedAt);
        Assert.Equal(1, alert.ResolvedByUserId);
    }

    [Fact]
    public void Acknowledge_Twice_RejectsTransition()
    {
        var alert = CreateAlert();
        alert.Acknowledge();

        Assert.Throws<InvalidOperationException>(alert.Acknowledge);
    }

    private static Alert CreateAlert() => new("ALT-001", 1, 1, AlertType.HighTemperature,
        AlertSeverity.Warning, "Temperature is high.", 9m, 8m);
}
