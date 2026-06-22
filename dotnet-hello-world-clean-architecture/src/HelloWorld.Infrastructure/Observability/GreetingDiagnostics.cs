using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace HelloWorld.Infrastructure.Observability;

public static class GreetingDiagnostics
{
    public const string ActivitySourceName = "HelloWorld.Greetings";
    public const string MeterName = "HelloWorld.Greetings";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
    public static readonly Meter Meter = new(MeterName);

    public static readonly Counter<long> GreetingsCreated = Meter.CreateCounter<long>(
        "hello_world.greetings.created",
        unit: "{greeting}",
        description: "Number of greetings successfully created.");

    public static readonly Counter<long> GreetingRequestsRejected = Meter.CreateCounter<long>(
        "hello_world.greetings.rejected",
        unit: "{request}",
        description: "Number of greeting requests rejected by validation.");

    public static readonly Counter<long> GreetingHistoryRequests = Meter.CreateCounter<long>(
        "hello_world.greetings.history.requests",
        unit: "{request}",
        description: "Number of greeting history requests handled.");
}
