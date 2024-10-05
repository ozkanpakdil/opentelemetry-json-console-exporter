using System.Diagnostics;
using System.Text.Json;
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

    private static Dictionary<string, string>? ConvertDictionary(IEnumerable<KeyValuePair<string, object?>> dictionary)
    {
        return dictionary?.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value?.ToString() ?? string.Empty) ?? new Dictionary<string, string>();
    }

    public override ExportResult Export(in Batch<Activity> batch)
    {
        foreach (var activity in batch)
        {
            var output = new ActivityOutput
            {
                Activity = new ActivityInfo
                {
                    TraceId = activity.TraceId.ToString(),
                    SpanId = activity.SpanId.ToString(),
                    ActivityTraceFlags = activity.ActivityTraceFlags,
                    TraceStateString = activity.TraceStateString,
                    ParentSpanId = activity.ParentSpanId.ToString(),
                    ActivitySourceName = activity.Source.Name,
                    ActivitySourceVersion = activity.Source.Version,
                    DisplayName = activity.DisplayName,
                    Kind = activity.Kind,
                    StartTime = activity.StartTimeUtc.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"),
                    Duration = activity.Duration,
                    Tags = ConvertDictionary(activity.TagObjects),
                    StatusCode = activity.Status,
                    StatusDescription = activity.StatusDescription,
                    Events = activity.Events.Select(e => new ActivityEvent
                    {
                        Name = e.Name,
                        Timestamp = e.Timestamp,
                        Attributes = ConvertDictionary(e.Tags!)
                    }).ToList(),
                    Links = activity.Links.Select(l => new ActivityLink
                    {
                        TraceId = l.Context.TraceId.ToString(),
                        SpanId = l.Context.SpanId.ToString(),
                        Attributes = ConvertDictionary(l.Tags!)
                    }).ToList(),
                    Resource = ParentProvider.GetResource() != Resource.Empty
                        ? ConvertDictionary(ParentProvider.GetResource().Attributes!)
                        : null,
                    RootId = activity.RootId,
                    OperationName = activity.OperationName,
                }
            };

            var json = JsonSerializer.Serialize(output, ActivityOutputContext.Default.ActivityOutput);

            if (_options != null)
                WriteLine(json);
            else
                Trace.WriteLine(json);
        }

        return ExportResult.Success;
    }
}