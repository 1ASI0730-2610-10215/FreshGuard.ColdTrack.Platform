using FreshGuard.ColdTrack.Platform.Shared.Application.Model;

namespace FreshGuard.ColdTrack.Platform.Analytics.Application.QueryServices;

public record ReportFile(string FileName, byte[] Content);

public interface IReportFileService
{
    Task<Result<ReportFile>> GenerateAsync(int reportId, CancellationToken cancellationToken);
}
