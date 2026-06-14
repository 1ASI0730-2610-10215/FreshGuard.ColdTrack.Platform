using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Services;

namespace FreshGuard.ColdTrack.Platform.Tests.Alerting.Domain;

public class ThresholdPolicyTests
{
    private readonly ThresholdPolicy _policy = new(2m, 8m, 60m);

    [Fact]
    public void Evaluate_WithNormalReading_ReturnsNoViolations() =>
        Assert.Empty(_policy.Evaluate(5m, 50m));

    [Fact]
    public void Evaluate_WithCriticalTemperature_ReturnsCriticalViolation()
    {
        var violation = Assert.Single(_policy.Evaluate(11m, 50m));

        Assert.Equal(AlertType.HighTemperature, violation.Type);
        Assert.Equal(AlertSeverity.Critical, violation.Severity);
    }

    [Fact]
    public void Evaluate_WithTemperatureAndHumidityViolations_ReturnsBoth() =>
        Assert.Equal(2, _policy.Evaluate(9m, 70m).Count());
}
