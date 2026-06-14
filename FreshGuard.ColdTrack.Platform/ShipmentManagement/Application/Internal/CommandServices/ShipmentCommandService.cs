using FreshGuard.ColdTrack.Platform.Shared.Application.Model;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Resources;
using Microsoft.Extensions.Localization;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.Internal.CommandServices;

public class ShipmentCommandService(IShipmentRepository repository, IUnitOfWork unitOfWork,
    IStringLocalizer<ShipmentMessages> localizer) : IShipmentCommandService
{
    public async Task<Result<Shipment>> Handle(CreateShipmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var sequence = await repository.GetNextCodeSequenceAsync(cancellationToken);
            var shipment = new Shipment($"ENV-{sequence:000}", command.Destination, command.DriverId,
                command.CargoDescription, command.DepartureDate, command.EstimatedArrival);
            await repository.AddAsync(shipment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Shipment>.Success(shipment);
        }
        catch (ArgumentException exception)
        {
            return Result<Shipment>.Failure(ShipmentError.InvalidShipmentData, exception.Message);
        }
    }

    public async Task<Result<Shipment>> Handle(UpdateShipmentStatusCommand command, CancellationToken cancellationToken)
    {
        var shipment = await repository.FindByIdAsync(command.ShipmentId, cancellationToken);
        if (shipment is null)
            return Result<Shipment>.Failure(ShipmentError.ShipmentNotFound, localizer["ShipmentNotFound"]);
        if (!TryParseStatus(command.Status, out var status))
            return Result<Shipment>.Failure(ShipmentError.InvalidShipmentStatus, localizer["InvalidShipmentStatus"]);

        try
        {
            shipment.ChangeStatus(status, command.ChangedByUserId, command.Remarks);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Shipment>.Success(shipment);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Shipment>.Failure(ShipmentError.InvalidStatusTransition, exception.Message);
        }
    }

    private static bool TryParseStatus(string value, out ShipmentStatus status) =>
        Enum.TryParse(value.Replace("_", string.Empty), true, out status);
}
