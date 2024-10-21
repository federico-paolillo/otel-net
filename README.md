# OpenTelemetry with .NET

The tiniest example I could think of. Good as template.

## Run

```shell
docker compose up
dotnet run --project Tel.Web
```

## Features

- `Microsoft.Extensions.Logging` based logging without clutter on `stdout`
- Application startup logging. Before initializing OpenTelemetry
- [`OTEL_DIAGNOSTICS.json`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry/README.md#self-diagnostics) file included
- ASP .NET Instrumentation plus custom explicit tracing
- [Grafana LGTM](https://github.com/grafana/docker-otel-lgtm) included as observability platform
- [OpenTelemetry Collector Contrib.](https://github.com/open-telemetry/opentelemetry-collector-contrib) with configuration and self-monitoring included
- ASP .NET minimal APIs