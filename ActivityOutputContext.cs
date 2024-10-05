using System.Diagnostics;
using System.Text.Json.Serialization;
using OpenTelemetry.Trace;

namespace Ozkanpakdil.OpenTelemetry.Exporter.Json.Console;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(ActivityOutput))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(float))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(ActivityKind))]
[JsonSerializable(typeof(ActivityTraceFlags))]
[JsonSerializable(typeof(Status))]
[JsonSerializable(typeof(ActivityStatusCode))]
internal partial class ActivityOutputContext : JsonSerializerContext;

public class ActivityOutput
{
    public ActivityInfo? Activity { get; set; }
}

public class ActivityInfo
{
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter<ActivityTraceFlags>))]
    public ActivityTraceFlags ActivityTraceFlags { get; set; }

    public string? TraceStateString { get; set; }
    public string? ParentSpanId { get; set; }
    public string? ActivitySourceName { get; set; }
    public string? ActivitySourceVersion { get; set; }
    public string? DisplayName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter<ActivityKind>))]
    public ActivityKind Kind { get; set; }

    public string? StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public Dictionary<string, string>? Tags { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter<ActivityStatusCode>))]
    public ActivityStatusCode StatusCode { get; set; }

    public string? StatusDescription { get; set; }
    public List<ActivityEvent>? Events { get; set; }
    public List<ActivityLink>? Links { get; set; }
    public Dictionary<string, string>? Resource { get; set; }
    public string? RootId { get; set; }
    public string? OperationName { get; set; }
}

public class ActivityEvent
{
    public string? Name { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public Dictionary<string, string>? Attributes { get; set; }
}

public class ActivityLink
{
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
    public Dictionary<string, string>? Attributes { get; set; }
}