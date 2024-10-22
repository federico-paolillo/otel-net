using Microsoft.Extensions.Logging;

namespace Tel.Instrument.Extensions;

internal static partial class LogMessages
{
    [LoggerMessage(LogLevel.Information, "Logs will be exported to {Uri}")]
    public static partial void LogsExportedAt(this ILogger logger, Uri uri);

    [LoggerMessage(LogLevel.Information, "Traces will be exported to {Uri}")]
    public static partial void TracesExportedAt(this ILogger logger, Uri uri);

    [LoggerMessage(LogLevel.Information, "Metrics will be exported to {Uri}")]
    public static partial void MetricsExportedAt(this ILogger logger, Uri uri);
}