# Console Json Exporter for OpenTelemetry .NET

[![NuGet](https://img.shields.io/nuget/v/Ozkanpakdil.OpenTelemetry.Exporter.Json.Console.svg)](https://www.nuget.org/packages/Ozkanpakdil.OpenTelemetry.Exporter.Json.Console)
[![NuGet](https://img.shields.io/nuget/dt/Ozkanpakdil.OpenTelemetry.Exporter.Json.Console.svg)](https://www.nuget.org/packages/Ozkanpakdil.OpenTelemetry.Exporter.Json.Console)

The console exporter prints data to the debug Console window.
ConsoleExporter supports exporting logs, metrics and traces.

> [!WARNING]
> This component is intended to be used while learning how telemetry data is
  created and exported. It is not recommended for any production environment.

## Installation

```shell
dotnet add package Ozkanpakdil.OpenTelemetry.Exporter.Json.Console
```

## Configuration

You can configure the `ConsoleExporter` through `Options` types properties
and environment variables.
The `Options` type setters take precedence over the environment variables.

## Environment Variables

The following environment variables can be used to override the default
values of the `PeriodicExportingMetricReaderOptions`
(following the [OpenTelemetry specification](https://github.com/open-telemetry/opentelemetry-specification/blob/v1.12.0/specification/sdk-environment-variables.md#periodic-exporting-metricreader).

| Environment variable          | `PeriodicExportingMetricReaderOptions` property |
| ------------------------------| ------------------------------------------------|
| `OTEL_METRIC_EXPORT_INTERVAL` | `ExportIntervalMilliseconds`                    |
| `OTEL_METRIC_EXPORT_TIMEOUT`  | `ExportTimeoutMilliseconds`                     |

## References

* [OpenTelemetry Project](https://opentelemetry.io/)
