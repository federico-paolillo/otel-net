# OpenTelemetry with .NET

The tiniest local-learning example I could think of.

## Run

```shell
docker compose up
dotnet run --project Tel.Web
```

Grafana will be available (after a short while) on `localhost:40300`. Use `admin` as username and password. You can use [JetBrains HTTP Client](https://www.jetbrains.com/help/idea/http-client-in-product-code-editor.html) to simulate traffic by running the call defined in `Tel.Web.http`.

## Features

- `Microsoft.Extensions.Logging` based logging without clutter on `stdout`
- Application startup logging. Before initializing OpenTelemetry
- [`OTEL_DIAGNOSTICS.json`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry/README.md#self-diagnostics) file included
- ASP .NET Instrumentation plus custom explicit tracing
- [Grafana LGTM](https://github.com/grafana/docker-otel-lgtm) included as observability platform
- [OpenTelemetry Collector Contrib.](https://github.com/open-telemetry/opentelemetry-collector-contrib) with configuration and self-monitoring included
- ASP .NET minimal APIs
- Source code commentary with links to reading resources
- _TODO_ Exemplars
- _TODO_ Samplers

## Pictures

_Human readable logs on `stdout`_

![Human Readable Logs on stdout](imgs/logs.png)

_Application traces_

![Application traces](imgs/tempo.png)

_Application logs_

![Application logs](imgs/loki.png)

_Application metrics_

![Application metrics](imgs/prom.png)

_OpenTelemetry Collector metrics_

![Application metrics](imgs/otelcol.png)

## Side notes

### Pitfalls

This example uses OTLP over plain HTTP/protobuf because it keeps local traffic easy to inspect. Both `http/protobuf` and `grpc` are valid OTLP transports; production choices should be based on TLS, authentication, backend support, and deployment topology. [See the official OTLP exporter configuration documentation](https://opentelemetry.io/docs/concepts/sdk-configuration/otlp-exporter-configuration/).

Standard ASP.NET Core HTTP metrics are intentionally not registered in this example to de-noise the output and keep attention on one custom metric. [See the official OpenTelemetry .NET metrics documentation](https://opentelemetry.io/docs/languages/dotnet/metrics/) for normal metrics instrumentation.

The explicit Collector pipelines intentionally omit [`batch`](https://github.com/open-telemetry/opentelemetry-collector/blob/main/processor/batchprocessor/README.md) and [`memory_limiter`](https://github.com/open-telemetry/opentelemetry-collector/blob/main/processor/memorylimiterprocessor/README.md) processors to make local signal visibility fast. These processors are normal production hardening components. [See the official Collector processor documentation](https://opentelemetry.io/docs/collector/components/processor/).

There are two OpenTelemetry Collectors: one baked in the Grafana LGTM container and one configured explicitly. This setup is for demonstration only. In production, use one deliberate Collector topology. [See the official Collector configuration documentation](https://opentelemetry.io/docs/collector/configuration/).

The `debug` exporter and `DEBUG` Collector logs are useful for learning, but they are noisy and should not be copied as production defaults. [See the official Collector exporter documentation](https://opentelemetry.io/docs/collector/components/exporter/).

The LGTM container has no persistence volume and this compose file has no health checks. That keeps the demo small, but telemetry data is ephemeral and readiness is verified manually. [See the official Grafana Docker OpenTelemetry LGTM documentation](https://grafana.com/docs/opentelemetry/docker-lgtm/).

### Seemingly double metrics

You will see a job named `opentelemetry-collector` in Prometheus metrics. This exists because:
> By default, the Collector generates basic metrics about itself and exposes them using the OpenTelemetry Go Prometheus exporter for scraping at http://127.0.0.1:8888/metrics. [See here for mor information on default metrics](https://opentelemetry.io/docs/collector/internal-telemetry/#configure-internal-metrics)

The Grafana LGTM distribution used in this example sets up scraping on `localhost:8888` by default. [See here the Grafana LGTM OpenTelemetry Collector configuration](https://github.com/grafana/docker-otel-lgtm/blob/b2bc9f79fab0b3eb6b9fa18ac817d8f1983c058d/docker/otelcol-config.yaml#L13). The metrics for the OpenTelemetry Collector used within this example are under job name `otelcol-contrib`.

### OpenTelemetry Configuration Schema

To better understand how OpenTelemetry `service` stanza can be configured browse the [Go source code type definitions here](https://pkg.go.dev/go.opentelemetry.io/collector/service#Config).

### OTEL_DIAGNOSTICS.json

Configures self-diagnostic for OpenTelemetry .NET SDK, the file is supposed to be full of `NUL` because it is pre-allocated. [See here for further details](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry/README.md#self-diagnostics)

### How do I change the labels that Loki assigns to my OTLP logs ?

[See the official Loki documentation](https://grafana.com/docs/loki/latest/send-data/otel/#changing-the-default-mapping-of-otlp-to-loki-format)

### Do I have the same concept for labels in Tempo, like Loki ?

No.
