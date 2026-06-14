using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Aggregates;

namespace FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Model.Queries;

public record GetAllShipmentsQuery(ShipmentStatus? Status);
