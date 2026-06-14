using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Aggregates;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.Analytics.Application.OutboundServices;

public interface IPdfReportGenerator
{
    byte[] Generate(Report report, IReadOnlyCollection<HistoricalLog> history);
}
