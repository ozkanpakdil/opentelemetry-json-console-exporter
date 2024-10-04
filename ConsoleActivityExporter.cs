// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;

namespace Ozkanpakdil.OpenTelemetry.Exporter.Json.Console;

/// <summary>
/// This is another Console exporter for OpenTelemetry.
/// The difference from regular console exporter is that it generates JSON for easy parsing.
/// Mainly used for debugging. And not suggested in production.
/// Check https://github.com/open-telemetry/opentelemetry-dotnet/pull/5588#issuecomment-2387161798 for more details. 
/// </summary>
public class ConsoleActivityExporter(ConsoleExporterOptions options) : ConsoleExporter<Activity>(options)
{
    private readonly ConsoleExporterOptions _options = options;

    public override ExportResult Export(in Batch<Activity> batch)
    {
        foreach (var activity in batch)
        {
            var output = new
            {
                Activity = new
                {
                    TraceId = activity.TraceId.ToString(),
                    SpanId = activity.SpanId.ToString(),
                    TraceFlags = activity.ActivityTraceFlags,
                    TraceState = string.IsNullOrEmpty(activity.TraceStateString) ? null : activity.TraceStateString,
                    ParentSpanId = activity.ParentSpanId == default ? default : activity.ParentSpanId.ToString(),
                    ActivitySourceName = activity.Source.Name,
                    ActivitySourceVersion =
                        string.IsNullOrEmpty(activity.Source.Version) ? null : activity.Source.Version,
                    activity.DisplayName,
                    activity.Kind,
                    StartTime = activity.StartTimeUtc.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"),
                    activity.Duration,
                    Tags = activity.TagObjects.Select(tag => new { tag.Key, tag.Value }).ToList(),
                    StatusCode = activity.Status != ActivityStatusCode.Unset ? activity.Status.ToString() : null,
                    StatusDescription =
                        !string.IsNullOrEmpty(activity.StatusDescription) ? activity.StatusDescription : null,
                    Events = activity.Events.Any()
                        ? null
                        : activity.Events.Select(e => new
                        {
                            e.Name,
                            e.Timestamp,
                            Attributes = e.Tags.Select(a => new { a.Key, a.Value }).ToList(),
                        }).ToList(),
                    Links = activity.Links.Any()
                        ? null
                        : activity.Links.Select(l =>
                        {
                            if (l.Tags != null)
                            {
                                return new
                                {
                                    l.Context.TraceId,
                                    l.Context.SpanId,
                                    Attributes = l.Tags.Select(a => new { a.Key, a.Value }).ToList(),
                                };
                            }

                            return null;
                        }).ToList(),
                    Resource = ParentProvider.GetResource() != Resource.Empty
                        ? ParentProvider.GetResource().Attributes.Select(a => new { a.Key, a.Value })
                            .ToList()
                        : null,
                },
            };

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
#if NETSTANDARD || NETFRAMEWORK
    IgnoreNullValues = true,
#else
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
#endif
                Converters = { new JsonStringEnumConverter(), },
                TypeInfoResolver = new JsonTypeInfoResolver(),
            };

            var json = JsonSerializer.Serialize(output, jsonSerializerOptions);

            if (_options != null)
                WriteLine(json);
            else // if no options provided, printing to debug console
                Trace.WriteLine(json);
        }

        return ExportResult.Success;
    }
}

public class JsonTypeInfoResolver : IJsonTypeInfoResolver
{
    private readonly DefaultJsonTypeInfoResolver _defaultResolver = new();

    public JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var typeInfo = _defaultResolver.GetTypeInfo(type, options);

        // If the type is a TimeSpan, customize its serialization, maybe in the future
        // if (type == typeof(TimeSpan))
        // {
        //     typeInfo.CustomConverter = new JsonStringConverter<TimeSpan>(
        //         timeSpan => timeSpan.ToString("c"),
        //         str => TimeSpan.Parse(str));
        // }
        return typeInfo;
    }
}