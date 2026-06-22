using HelloWorld.Api.Security;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace HelloWorld.UnitTests.Api;

internal static class HelloWorldApiFactory
{
    public const string TestApiKey = "test-api-key";

    public static WebApplicationFactory<Program> Create(params string[] scopes)
    {
        var configuredScopes = scopes.Length == 0
            ? [AuthorizationPolicies.GreetingReadScope, AuthorizationPolicies.GreetingHistoryReadScope]
            : scopes;

        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("environment", "Production");
                builder.ConfigureAppConfiguration((_, configuration) =>
                {
                    configuration.Sources.Clear();

                    var values = new Dictionary<string, string?>
                    {
                        ["Authentication:ApiKey:ApiKey"] = TestApiKey,
                        ["OpenTelemetry:UseConsoleExporter"] = "false",
                        ["OpenTelemetry:UseOtlpExporter"] = "false"
                    };

                    for (var i = 0; i < configuredScopes.Length; i++)
                    {
                        values[$"Authentication:ApiKey:Scopes:{i}"] = configuredScopes[i];
                    }

                    configuration.AddInMemoryCollection(values);
                });
            });
    }
}
