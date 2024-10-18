using System.Diagnostics;

namespace Tel.Instrument;

public sealed class Instrumentation : IDisposable
{
    // You need a well-known Activity Source name to add to OpenTelemetry Trace Provider
    // By default, OpenTelemetry will not listen to any Activity Source
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace#common-issues-that-lead-to-missing-traces
    // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace/customizing-the-sdk#activity-source
    
    public const string SourceName = "Tel.Web";
    
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

    public Instrumentation()
    {
        // It is recommended to have a very long living Activity Source per application
        // See: https://github.com/open-telemetry/opentelemetry-dotnet/tree/cddc09127f49143de1dc54800b65b710c1f6056d/docs/trace#activitysource
        // See: https://opentelemetry.io/docs/languages/net/instrumentation/#setting-up-an-activitysource
        
        ActivitySource = new ActivitySource(SourceName);
    }
    
    public void Dispose()
    {
        ActivitySource.Dispose();
    }
}