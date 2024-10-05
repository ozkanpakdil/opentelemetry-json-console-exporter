using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace Ozkanpakdil.OpenTelemetry.Exporter.Json.Console;

public static class ConsoleExporterHelperExtensions
{
    /// <summary>
    /// Adds Console exporter to the TracerProvider.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddJsonConsoleExporter(this TracerProviderBuilder builder)
        => AddJsonConsoleExporter(builder, name: null, configure: null);

    /// <summary>
    /// Adds Console exporter to the TracerProvider.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
    /// <param name="configure">Callback action for configuring <see cref="ConsoleExporterOptions"/>.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddJsonConsoleExporter(this TracerProviderBuilder builder,
        Action<ConsoleExporterOptions> configure)
        => AddJsonConsoleExporter(builder, name: null, configure);

    /// <summary>
    /// Adds Console exporter to the TracerProvider.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
    /// <param name="name">Optional name which is used when retrieving options.</param>
    /// <param name="configure">Optional callback action for configuring <see cref="ConsoleExporterOptions"/>.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    private static TracerProviderBuilder AddJsonConsoleExporter(
        this TracerProviderBuilder builder,
        string? name,
        Action<ConsoleExporterOptions>? configure)
    {
        ArgumentNullException.ThrowIfNull(builder);

        name ??= Options.DefaultName;

        if (configure != null)
        {
            builder.ConfigureServices(services => services.Configure(name, configure));
        }

        return builder.AddProcessor(sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<ConsoleExporterOptions>>().Get(name);

            return new SimpleActivityExportProcessor(new ConsoleActivityExporter(options));
        });
    }
}