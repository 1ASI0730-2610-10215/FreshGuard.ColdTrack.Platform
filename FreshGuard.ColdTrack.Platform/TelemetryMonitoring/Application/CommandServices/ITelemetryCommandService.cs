using FreshGuard.ColdTrack.Platform.Shared.Application.Model;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.CommandServices;

public interface ITelemetryCommandService
{
    Task<Result<Sensor>> Handle(RegisterSensorCommand command, CancellationToken cancellationToken);
    Task<Result<Sensor>> Handle(AssignSensorCommand command, CancellationToken cancellationToken);
    Task<Result<TelemetryLog>> Handle(RecordTelemetryCommand command, CancellationToken cancellationToken);
}
