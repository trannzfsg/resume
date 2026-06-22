using System.Net;
using System.Net.Http.Json;
using HelloWorld.Api.Security;

namespace HelloWorld.UnitTests.Api;

public sealed class ApiSecurityTests
{
    [Fact]
    public async Task Anonymous_endpoints_do_not_require_api_key()
    {
        using var factory = HelloWorldApiFactory.Create();
        using var client = factory.CreateClient();

        var rootResponse = await client.GetAsync("/");
        var healthResponse = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, rootResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, healthResponse.StatusCode);
    }

    [Fact]
    public async Task Greeting_endpoint_returns_unauthorized_without_api_key()
    {
        using var factory = HelloWorldApiFactory.Create();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/greetings?name=Tran");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Greeting_endpoint_returns_unauthorized_for_wrong_api_key()
    {
        using var factory = HelloWorldApiFactory.Create();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(ApiKeyAuthenticationDefaults.DefaultHeaderName, "wrong-key");

        var response = await client.GetAsync("/api/greetings?name=Tran");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Greeting_endpoint_returns_unauthorized_when_multiple_api_keys_are_provided()
    {
        using var factory = HelloWorldApiFactory.Create();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(
            ApiKeyAuthenticationDefaults.DefaultHeaderName,
            [HelloWorldApiFactory.TestApiKey, HelloWorldApiFactory.TestApiKey]);

        var response = await client.GetAsync("/api/greetings?name=Tran");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Greeting_endpoint_accepts_valid_api_key_with_required_scope()
    {
        using var factory = HelloWorldApiFactory.Create();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(
            ApiKeyAuthenticationDefaults.DefaultHeaderName,
            HelloWorldApiFactory.TestApiKey);

        var response = await client.GetFromJsonAsync<GreetingApiResponse>("/api/greetings?name=Tran");

        Assert.NotNull(response);
        Assert.Equal("Tran", response.Recipient);
        Assert.Equal("Hello, Tran!", response.Message);
    }

    [Fact]
    public async Task Greeting_history_endpoint_returns_forbidden_when_api_key_lacks_history_scope()
    {
        using var factory = HelloWorldApiFactory.Create(AuthorizationPolicies.GreetingReadScope);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(
            ApiKeyAuthenticationDefaults.DefaultHeaderName,
            HelloWorldApiFactory.TestApiKey);

        var response = await client.GetAsync("/api/greetings/history?max=10");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
