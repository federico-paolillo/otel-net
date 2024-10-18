using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Tel.Instrument;
using Tel.Web.Extensions;

namespace Tel.Web;

public static class OpenTelemetry
{
    public static WebApplicationBuilder UseOpenTelemetry(
        this WebApplicationBuilder builder,
        ILogger logger
    )
    {
        Configuration.OpenTelemetry? cfg = builder.Configuration
            .GetSection(Configuration.OpenTelemetry.Section)
            .Get<Configuration.OpenTelemetry>();

        if (cfg is null)
        {
            throw new InvalidOperationException("Missing OpenTelemetry configuration");
        }

        // OTLP Exporter endpoints should contain /v1/logs,traces,metrics
        // When touching OltpExporterOptions.Endpoint it will not be added automatically
        // See: https://opentelemetry.io/docs/languages/net/exporters/#aspnet-core
        // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/0343715f49ac8e121ec39acd92f8d5572b3d036d/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/IOtlpExporterOptions.cs#L40

        logger.LogsExportedAt(cfg.Logging.Endpoint);
        logger.TracesExportedAt(cfg.Tracing.Endpoint);

        // See: https://opentelemetry.io/docs/languages/net/instrumentation/#initialize-the-sdk

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(bld =>
            {
                // Omnipresent and immutable attributes and service identity should be modeled as a Resource
                // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/trace/customizing-the-sdk/README.md#resource
                // See: https://opentelemetry.io/docs/specs/otel/resource/sdk/ 
                
                bld.AddService("Tel.Web");
            })
            // This will ensure registration of a LoggingProvider plugged into OpenTelemetry
            // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/cddc09127f49143de1dc54800b65b710c1f6056d/docs#initialize-the-sdk-using-a-host
            .WithLogging(bld =>
            {
                bld.AddOtlpExporter(opts =>
                {
                    opts.ExportProcessorType = ExportProcessorType.Simple;
                    opts.Protocol = OtlpExportProtocol.HttpProtobuf;
                    opts.Endpoint = cfg.Logging.Endpoint;
                    opts.TimeoutMilliseconds = cfg.Logging.ExportTimeout;
                });
            }, opts =>
            {
                // Including scopes will make the T in ILogger<T> visible in the logs
                // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/cddc09127f49143de1dc54800b65b710c1f6056d/docs/logs/customizing-the-sdk#includescopes

                opts.IncludeScopes = true;
            })
            .WithTracing(bld =>
            {
                bld.AddAspNetCoreInstrumentation(opts =>
                {
                    opts.RecordException = true;
                }).AddOtlpExporter(opts =>
                {
                    opts.ExportProcessorType = ExportProcessorType.Simple;
                    opts.Protocol = OtlpExportProtocol.HttpProtobuf;
                    opts.Endpoint = cfg.Tracing.Endpoint;
                    opts.TimeoutMilliseconds = cfg.Tracing.ExportTimeout;
                })
                .AddSource(Instrumentation.SourceName)
                .SetSampler(new AlwaysOnSampler());
            });

        builder.Services.AddSingleton<Instrumentation>();

        return builder;
    }
}