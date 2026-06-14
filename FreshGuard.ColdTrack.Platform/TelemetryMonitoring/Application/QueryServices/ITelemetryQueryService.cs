using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Queries;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.QueryServices;

public interface ITelemetryQueryService
{
    Task<IEnumerable<Sensor>> Handle(GetAllSensorsQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<TelemetryLog>> Handle(GetTelemetryByShipmentIdQuery query, CancellationToken cancellationToken);
}
