namespace Tel.Web.Configuration;

public sealed record OpenTelemetry(Logging Logging)
{
    public const string Section = "OpenTelemetry";
}

public sealed record Logging
{
    public Uri Endpoint { get; init; } = new("http://localhost:4318");

    public int ExportTimeout { get; init; } = 1000;
}