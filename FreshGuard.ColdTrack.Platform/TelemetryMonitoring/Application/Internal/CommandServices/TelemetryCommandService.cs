using FreshGuard.ColdTrack.Platform.Shared.Application.Model;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Entities;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Model.Errors;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.Internal.CommandServices;

public class TelemetryCommandService(ISensorRepository sensorRepository, ITelemetryRepository telemetryRepository,
    IShipmentRepository shipmentRepository, IUnitOfWork unitOfWork) : ITelemetryCommandService
{
    public async Task<Result<Sensor>> Handle(RegisterSensorCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var sensor = new Sensor(command.SensorCode, command.ModelName);
            if (await sensorRepository.ExistsByCodeAsync(sensor.SensorCode, cancellationToken))
                return Result<Sensor>.Failure(TelemetryError.SensorCodeAlreadyExists,
                    "The sensor code already exists.");

            await sensorRepository.AddAsync(sensor, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Sensor>.Success(sensor);
        }
        catch (ArgumentException exception)
        {
            return Result<Sensor>.Failure(TelemetryError.InvalidTelemetry, exception.Message);
        }
    }

    public async Task<Result<Sensor>> Handle(AssignSensorCommand command, CancellationToken cancellationToken)
    {
        var sensor = await sensorRepository.FindByIdAsync(command.SensorId, cancellationToken);
        if (sensor is null)
            return Result<Sensor>.Failure(TelemetryError.SensorNotFound, "The sensor was not found.");
        if (await shipmentRepository.FindByIdAsync(command.ShipmentId, cancellationToken) is null)
            return Result<Sensor>.Failure(TelemetryError.ShipmentNotFound, "The shipment was not found.");

        try
        {
            sensor.AssignToShipment(command.ShipmentId);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Sensor>.Success(sensor);
        }
        catch (Exception exception) when (exception is ArgumentOutOfRangeException or InvalidOperationException)
        {
            return Result<Sensor>.Failure(TelemetryError.SensorUnavailable, exception.Message);
        }
    }

    public async Task<Result<TelemetryLog>> Handle(RecordTelemetryCommand command,
        CancellationToken cancellationToken)
    {
        var sensor = await sensorRepository.FindByIdAsync(command.SensorId, cancellationToken);
        if (sensor is null)
            return Result<TelemetryLog>.Failure(TelemetryError.SensorNotFound, "The sensor was not found.");
        if (sensor.ShipmentId is null)
            return Result<TelemetryLog>.Failure(TelemetryError.SensorUnavailable,
                "The sensor is not assigned to a shipment.");

        try
        {
            sensor.RecordReading(command.Temperature, command.Humidity, command.RecordedAt);
            var log = new TelemetryLog(sensor.Id, sensor.ShipmentId.Value, command.Temperature, command.Humidity,
                command.RecordedAt);
            await telemetryRepository.AddAsync(log, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<TelemetryLog>.Success(log);
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
        {
            return Result<TelemetryLog>.Failure(TelemetryError.InvalidTelemetry, exception.Message);
        }
    }
}
