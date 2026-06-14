using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Queries;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.Internal.QueryServices;

public class TelemetryQueryService(ISensorRepository sensorRepository, ITelemetryRepository telemetryRepository)
    : ITelemetryQueryService
{
    public Task<IEnumerable<Sensor>> Handle(GetAllSensorsQuery query, CancellationToken cancellationToken) =>
        sensorRepository.ListAsync(cancellationToken);

    public Task<IEnumerable<TelemetryLog>> Handle(GetTelemetryByShipmentIdQuery query,
        CancellationToken cancellationToken) =>
        telemetryRepository.ListByShipmentIdAsync(query.ShipmentId, cancellationToken);
}
