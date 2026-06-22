using HelloWorld.Infrastructure.Observability;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace HelloWorld.Api.Observability;

public static class OpenTelemetryExtensions
{
    private const string DefaultServiceName = "HelloWorld.CleanArchitecture";
    private const string DefaultServiceVersion = "1.0.0";

    public static WebApplicationBuilder AddHelloWorldOpenTelemetry(this WebApplicationBuilder builder)
    {
        var options = OpenTelemetryOptions.From(builder.Configuration, builder.Environment);

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(
                serviceName: options.ServiceName,
                serviceVersion: options.ServiceVersion))
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(GreetingDiagnostics.ActivitySourceName)
                    .AddAspNetCoreInstrumentation(instrumentation =>
                    {
                        instrumentation.RecordException = true;
                    });

                if (options.UseConsoleExporter)
                {
                    tracing.AddConsoleExporter();
                }

                if (options.UseOtlpExporter)
                {
                    tracing.AddOtlpExporter();
                }
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddMeter(GreetingDiagnostics.MeterName)
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation();

                if (options.UseConsoleExporter)
                {
                    metrics.AddConsoleExporter();
                }

                if (options.UseOtlpExporter)
                {
                    metrics.AddOtlpExporter();
                }
            });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.ParseStateValues = true;
            logging.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                serviceName: options.ServiceName,
                serviceVersion: options.ServiceVersion));

            if (options.UseConsoleExporter)
            {
                logging.AddConsoleExporter();
            }

            if (options.UseOtlpExporter)
            {
                logging.AddOtlpExporter();
            }
        });

        return builder;
    }

    private sealed record OpenTelemetryOptions(
        string ServiceName,
        string ServiceVersion,
        bool UseConsoleExporter,
        bool UseOtlpExporter)
    {
        public static OpenTelemetryOptions From(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var serviceName = configuration["OpenTelemetry:ServiceName"];
            var serviceVersion = configuration["OpenTelemetry:ServiceVersion"];
            var useOtlpExporter = IsEnabled(configuration["OpenTelemetry:UseOtlpExporter"])
                || !string.IsNullOrWhiteSpace(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            return new OpenTelemetryOptions(
                string.IsNullOrWhiteSpace(serviceName) ? DefaultServiceName : serviceName,
                string.IsNullOrWhiteSpace(serviceVersion) ? DefaultServiceVersion : serviceVersion,
                IsEnabled(configuration["OpenTelemetry:UseConsoleExporter"], environment.IsDevelopment()),
                useOtlpExporter);
        }

        private static bool IsEnabled(string? value, bool fallback = false) =>
            bool.TryParse(value, out var parsed)
                ? parsed
                : fallback;
    }
}
