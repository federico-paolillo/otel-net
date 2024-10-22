using Microsoft.Extensions.Logging.Console;

namespace Tel.Web.Extensions;

public static class SimpleLogger
{
    public static void UseSimpleLogger(this ILoggingBuilder builder)
    {
        // It is a goal of this demo to offer simple logs to stdout and hide OTEL signals from sight

        builder.ClearProviders();
        builder.AddSimpleConsole(opts =>
        {
            opts.SingleLine = true;
            opts.IncludeScopes = false;
            opts.UseUtcTimestamp = true;
            opts.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
            opts.ColorBehavior = LoggerColorBehavior.Disabled;
        });
    }
}