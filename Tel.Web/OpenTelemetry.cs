using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace Tel.Web;

public static class OpenTelemetry
{
    public static WebApplicationBuilder UseOpenTelemetry(this WebApplicationBuilder builder)
    {
        Configuration.OpenTelemetry? cfg = builder.Configuration
            .GetSection(Configuration.OpenTelemetry.Section)
            .Get<Configuration.OpenTelemetry>();

        if (cfg is null)
        {
            throw new InvalidOperationException("Missing OpenTelemetry configuration");
        }

        // startupLogger.LogInformation("Logs will be exported to {Uri}", cfg.Logging.Endpoint);

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(bld =>
            {
                bld.AddService("Tel.Web");
            })
            .WithLogging(bld =>
            {
                bld.AddOtlpExporter(opts =>
                {
                    opts.ExportProcessorType = ExportProcessorType.Simple;
                    opts.Protocol = OtlpExportProtocol.HttpProtobuf;
                });
            });

        return builder;
    }
}