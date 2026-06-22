using System.Net;
using System.Net.Http.Json;
using HelloWorld.Api.Security;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.UnitTests.Api;

public sealed class GreetingsApiTests
{
    [Fact]
    public async Task Greeting_endpoint_returns_validation_problem_for_invalid_name()
    {
        using var factory = HelloWorldApiFactory.Create();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(
            ApiKeyAuthenticationDefaults.DefaultHeaderName,
            HelloWorldApiFactory.TestApiKey);

        var response = await client.GetAsync($"/api/greetings?name={new string('a', 81)}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("RecipientName.TooLong", problem.Detail);
        Assert.True(problem.Errors.ContainsKey("name"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public async Task Greeting_history_endpoint_returns_validation_problem_for_invalid_limit(
        int maximumCount)
    {
        using var factory = HelloWorldApiFactory.Create();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(
            ApiKeyAuthenticationDefaults.DefaultHeaderName,
            HelloWorldApiFactory.TestApiKey);

        var response = await client.GetAsync($"/api/greetings/history?max={maximumCount}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("ListGreetings.InvalidLimit", problem.Detail);
        Assert.True(problem.Errors.ContainsKey("max"));
    }

    [Fact]
    public async Task Greeting_history_endpoint_returns_recent_greetings_for_valid_api_key()
    {
        using var factory = HelloWorldApiFactory.Create();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(
            ApiKeyAuthenticationDefaults.DefaultHeaderName,
            HelloWorldApiFactory.TestApiKey);

        using var firstGreetingResponse = await client.GetAsync("/api/greetings?name=Tran");
        using var secondGreetingResponse = await client.GetAsync("/api/greetings?name=Alex");
        firstGreetingResponse.EnsureSuccessStatusCode();
        secondGreetingResponse.EnsureSuccessStatusCode();

        var response = await client.GetFromJsonAsync<GreetingHistoryApiResponse>(
            "/api/greetings/history?max=2");

        Assert.NotNull(response);
        Assert.Collection(
            response.Greetings,
            greeting => Assert.Equal("Alex", greeting.Recipient),
            greeting => Assert.Equal("Tran", greeting.Recipient));
    }
}
