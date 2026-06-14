using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Services;

public record ThresholdViolation(AlertType Type, AlertSeverity Severity, string Message, decimal Value, decimal Limit);

/// <summary>Evaluates temperature and humidity readings against configurable cold-chain thresholds.</summary>
public class ThresholdPolicy(decimal minimumTemperature, decimal maximumTemperature, decimal maximumHumidity)
{
    public IEnumerable<ThresholdViolation> Evaluate(decimal temperature, decimal humidity)
    {
        if (temperature < minimumTemperature)
            yield return new ThresholdViolation(AlertType.LowTemperature,
                temperature < minimumTemperature - 2 ? AlertSeverity.Critical : AlertSeverity.Warning,
                "Temperature is below the permitted range.", temperature, minimumTemperature);
        if (temperature > maximumTemperature)
            yield return new ThresholdViolation(AlertType.HighTemperature,
                temperature > maximumTemperature + 2 ? AlertSeverity.Critical : AlertSeverity.Warning,
                "Temperature is above the permitted range.", temperature, maximumTemperature);
        if (humidity > maximumHumidity)
            yield return new ThresholdViolation(AlertType.HighHumidity,
                humidity > maximumHumidity + 15 ? AlertSeverity.Critical : AlertSeverity.Warning,
                "Humidity is above the permitted range.", humidity, maximumHumidity);
    }
}
