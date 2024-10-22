namespace Tel.Web.Extensions;

internal static partial class LogMessages
{
    [LoggerMessage(LogLevel.Information, "Starting host...")]
    public static partial void HostStarting(this ILogger logger);

    [LoggerMessage(LogLevel.Warning, "Quitting host...")]
    public static partial void HostQuitting(this ILogger logger);

    [LoggerMessage(LogLevel.Critical, "Host has failed")]
    public static partial void HostFailure(this ILogger logger, Exception ex);
}