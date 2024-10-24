namespace Tel.Instrument.Configuration;

public sealed record OpenTelemetry(
    Logging Logging,
    Tracing Tracing,
    Metrics Metrics
)
{
    public const string Section = "OpenTelemetry";
}

public sealed record Logging
{
    public const string Section = "Logging";

    public Uri Endpoint { get; init; } = new("http://localhost:4318/v1/logs");

    public int ExportTimeout { get; init; } = 1000;
}

public sealed record Tracing
{
    public const string Section = "Tracing";

    public Uri Endpoint { get; init; } = new("http://localhost:4318/v1/traces");

    public int ExportTimeout { get; init; } = 1000;
}

public sealed record Metrics
{
    public const string Section = "Metrics";

    public Uri Endpoint { get; init; } = new("http://localhost:4318/v1/metrics");

    public int ExportTimeout { get; init; } = 1000;
}