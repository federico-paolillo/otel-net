using Microsoft.Extensions.Logging.Console;

namespace Tel.Web;

public static class SimpleLogger
{
    public static void UseSimpleLogger(this ILoggingBuilder builder)
    {
        builder.ClearProviders();
        builder.AddSimpleConsole(opts =>
        {
            opts.SingleLine = true;
            opts.IncludeScopes = false;
            opts.UseUtcTimestamp = true;
            opts.TimestampFormat = "yyyy-MM-dd HH:mm:ss";
            opts.ColorBehavior = LoggerColorBehavior.Disabled;
        });
    }
}