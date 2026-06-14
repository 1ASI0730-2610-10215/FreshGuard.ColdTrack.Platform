using FreshGuard.ColdTrack.Platform.Analytics.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Analytics.Application.OutboundServices;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Commands;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Shared.Application.Model;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;

namespace FreshGuard.ColdTrack.Platform.Analytics.Application.Internal.CommandServices;

public class ReportCommandService(IAnalyticsDataSource dataSource, IReportRepository repository,
    IUnitOfWork unitOfWork) : IReportCommandService
{
    public async Task<Result<Report>> Handle(GenerateReportCommand command, CancellationToken cancellationToken)
    {
        var history = await dataSource.GetHistoryAsync(command.Period, cancellationToken);
        var sequence = await repository.GetNextCodeSequenceAsync(cancellationToken);
        var temperatures = history.Where(row => row.AverageTemperature.HasValue)
            .Select(row => row.AverageTemperature!.Value).ToArray();
        var humidity = history.Where(row => row.AverageHumidity.HasValue)
            .Select(row => row.AverageHumidity!.Value).ToArray();
        var report = new Report($"RPT-{sequence:000}", command.Period, history.Count,
            history.Count(row => row.Status == "Completed"), history.Sum(row => row.AlertCount),
            temperatures.Length == 0 ? null : Math.Round(temperatures.Average(), 2),
            humidity.Length == 0 ? null : Math.Round(humidity.Average(), 2), command.GeneratedByUserId);
        await repository.AddAsync(report, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Report>.Success(report);
    }
}
