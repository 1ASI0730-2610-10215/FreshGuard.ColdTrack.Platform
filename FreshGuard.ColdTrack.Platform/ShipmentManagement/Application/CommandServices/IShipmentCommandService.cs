using FreshGuard.ColdTrack.Platform.Shared.Application.Model;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Commands;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.CommandServices;

public interface IShipmentCommandService
{
    Task<Result<Shipment>> Handle(CreateShipmentCommand command, CancellationToken cancellationToken);
    Task<Result<Shipment>> Handle(UpdateShipmentStatusCommand command, CancellationToken cancellationToken);
}
