# Console Json Exporter for OpenTelemetry .NET

[![NuGet](https://img.shields.io/nuget/v/Ozkanpakdil.OpenTelemetry.Exporter.Json.Console.svg)](https://www.nuget.org/packages/Ozkanpakdil.OpenTelemetry.Exporter.Json.Console)
[![NuGet](https://img.shields.io/nuget/dt/Ozkanpakdil.OpenTelemetry.Exporter.Json.Console.svg)](https://www.nuget.org/packages/Ozkanpakdil.OpenTelemetry.Exporter.Json.Console)

The console exporter prints data to the debug Console window.
ConsoleExporter supports exporting logs, metrics and traces.

> [!WARNING]
> This component is intended to be used while learning how telemetry data is
> created and exported. It is not recommended for any production environment.

## Installation

```shell
dotnet add package Ozkanpakdil.OpenTelemetry.Exporter.Json.Console
```

## Configuration

Check [this code](https://github.com/ozkanpakdil/opentelemetry-json-console-exporter/blob/main/demo/Program.cs) for
WebApplication it has this part below

```csharp
    if (Debugger.IsAttached)
    {
        builder.AddJsonConsoleExporter(options =>
        {
            options.Targets = ConsoleExporterOutputTargets.Debug;
        });
    }
```

which enables the json printer for debug logging. You can test it by running the demo project with debugger.

Check honeycomb.io config for parameters, I created it
from [free account](https://docs.honeycomb.io/get-started/start-building/application/) honeycomb provides
for testing.
They support dotnet java npm and have good documentation, easy. This is necessary for opentelemetry, I did not wanted to
install opentelemetry compliant server so I used honeycomb. Below you can see how it looks in rider debugger.

![image](https://github.com/user-attachments/assets/5f05df19-f23d-4d11-b0d6-68c3c2c5a958)


## Environment Variables

The following environment variables can be used to override the default
values of the `PeriodicExportingMetricReaderOptions`
(following
the [OpenTelemetry specification](https://github.com/open-telemetry/opentelemetry-specification/blob/v1.12.0/specification/sdk-environment-variables.md#periodic-exporting-metricreader).

| Environment variable          | `PeriodicExportingMetricReaderOptions` property |
|-------------------------------|-------------------------------------------------|
| `OTEL_METRIC_EXPORT_INTERVAL` | `ExportIntervalMilliseconds`                    |
| `OTEL_METRIC_EXPORT_TIMEOUT`  | `ExportTimeoutMilliseconds`                     |

## References

* [OpenTelemetry Project](https://opentelemetry.io/)
