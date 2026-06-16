namespace FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Configuration;

/// <summary>
/// Loads local environment variables from the project environment file before the application configuration is built.
/// </summary>
/// <author>Codex OpenAI</author>
public static class EnvFileLoader
{
    private const string PreferredFileName = "FreshGuard-ColdTrack-Platform.env";
    private const string FallbackFileName = ".env";

    /// <summary>
    /// Searches for the project environment file from the current directory upward and loads each key-value pair.
    /// Existing environment variables are preserved to avoid overriding Render configuration.
    /// </summary>
    public static void Load()
    {
        var envPath = FindEnvFile();
        if (envPath is null) return;

        foreach (var rawLine in File.ReadAllLines(envPath))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#')) continue;

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0) continue;

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim().Trim('"');

            if (string.IsNullOrWhiteSpace(key) || Environment.GetEnvironmentVariable(key) is not null)
                continue;

            Environment.SetEnvironmentVariable(key, value);
        }
    }

    private static string? FindEnvFile()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, PreferredFileName);
            if (File.Exists(candidate)) return candidate;

            var fallback = Path.Combine(directory.FullName, FallbackFileName);
            if (File.Exists(fallback)) return fallback;

            directory = directory.Parent;
        }

        return null;
    }
}
