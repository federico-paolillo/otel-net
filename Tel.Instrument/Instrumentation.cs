using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Tel.Instrument;

public sealed class Instrumentation : IDisposable
{
    // You need a well-known Activity Source name to add to OpenTelemetry Trace Provider
    // By default, OpenTelemetry will not listen to any Activity Source
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace#common-issues-that-lead-to-missing-traces
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace/customizing-the-sdk#activity-source

    public const string SourceName = "Tel.Web";

    // In .NET, you must use the Metrics API to emit metrics. No shims are provided
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/README.md#package-version
    // See: https://learn.microsoft.com/en-us/dotnet/core/diagnostics/compare-metric-apis#systemdiagnosticsmetrics
    // The name of the meter is important. It must be explicitly registered when configuring metrics
    // By default, OpenTelemetry will not listen to any Meter
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/customizing-the-sdk/README.md#meter
    // Meters should be created at most once
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/README.md#meter

    private readonly Meter _meter = new(SourceName);

    public Instrumentation()
    {
        // It is recommended to have a very long living Activity Source per application
        // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/cddc09127f49143de1dc54800b65b710c1f6056d/docs/trace#activitysource
        // See: https://opentelemetry.io/docs/languages/net/instrumentation/#setting-up-an-activitysource

        ActivitySource = new ActivitySource(SourceName);

        // Instruments should be created at most once and kept around
        // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/README.md#instruments
        // Choose the best instrument according to the use case:
        // See: https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/metrics/supplementary-guidelines.md#instrument-selection
        // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/README.md#instruments
        // See: https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/metrics/api.md#general-operations
        // You should avoid invalid instrument names. OTEL will ignore them
        // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/README.md#instruments
        // See: https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/metrics/api.md#instrument-name-syntax

        WheaterRequests =
            _meter.CreateCounter<int>("wheater_requests", description: "number of wheater requests completed");
    }

    // In .NET, it is recommended to use the Activity API to Trace
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/cddc09127f49143de1dc54800b65b710c1f6056d/src/OpenTelemetry.Api#instrumenting-a-libraryapplication-with-net-activity-api
    // The Activity API is included with the correct version by OpenTelemetry libraries
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/cddc09127f49143de1dc54800b65b710c1f6056d/docs/trace#package-version
    // It is possible to use OpenTelemetry Tracing API Shim for .NET to use more OpenTelemetry-like APIs
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/cddc09127f49143de1dc54800b65b710c1f6056d/src/OpenTelemetry.Api#instrumenting-using-opentelemetryapi-shim
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/issues/947
    // Correlation is automagically made by OpenTelemetry
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/cddc09127f49143de1dc54800b65b710c1f6056d/docs/trace#correlation
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/cddc09127f49143de1dc54800b65b710c1f6056d/docs/logs/correlation/README.md

    public ActivitySource ActivitySource { get; }

    public Counter<int> WheaterRequests { get; }

    public void Dispose()
    {
        ActivitySource.Dispose();
        _meter.Dispose();
    }
}