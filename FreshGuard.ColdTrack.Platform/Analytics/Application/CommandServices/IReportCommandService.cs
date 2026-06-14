using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Shared.Application.Model;

namespace FreshGuard.ColdTrack.Platform.Analytics.Application.CommandServices;

public interface IReportCommandService
{
    Task<Result<Report>> Handle(GenerateReportCommand command, CancellationToken cancellationToken);
}
