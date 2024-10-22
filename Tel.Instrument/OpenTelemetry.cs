using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Tel.Instrument.Extensions;

namespace Tel.Instrument;

public static class OpenTelemetry
{
    public static IHostApplicationBuilder UseOpenTelemetry(
        this IHostApplicationBuilder builder,
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

        // OTLP Exporter endpoints should contain /v1/[logs,traces,metrics]
        // When touching OltpExporterOptions.Endpoint it will not be added automatically
        // See: https://opentelemetry.io/docs/languages/net/exporters/#aspnet-core
        // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/0343715f49ac8e121ec39acd92f8d5572b3d036d/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/IOtlpExporterOptions.cs#L40

        logger.LogsExportedAt(cfg.Logging.Endpoint);
        logger.TracesExportedAt(cfg.Tracing.Endpoint);
        logger.MetricsExportedAt(cfg.Metrics.Endpoint);

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
                    // You need a well-known Activity Source name to add to OpenTelemetry Trace Provider
                    // By default, OpenTelemetry will not listen to any Activity Source
                    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace#common-issues-that-lead-to-missing-traces
                    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace/customizing-the-sdk#activity-source
                    .AddSource(Instrumentation.SourceName)
                    .SetSampler(new AlwaysOnSampler());
            })
            .WithMetrics(bld =>
            {
                bld.AddOtlpExporter(opts =>
                    {
                        opts.ExportProcessorType = ExportProcessorType.Simple;
                        opts.Protocol = OtlpExportProtocol.HttpProtobuf;
                        opts.Endpoint = cfg.Metrics.Endpoint;
                        opts.TimeoutMilliseconds = cfg.Metrics.ExportTimeout;
                    })
                    // The name of the meter is important. It must be explicitly registered when configuring metrics
                    // By default, OpenTelemetry will not listen to any Meter
                    // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/customizing-the-sdk/README.md#meter
                    .AddMeter(Instrumentation.SourceName);
            });

        builder.Services.AddSingleton<Instrumentation>();

        return builder;
    }
}