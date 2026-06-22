namespace HelloWorld.UnitTests.Api;

internal sealed record GreetingApiResponse(
    Guid Id,
    string Recipient,
    string Message,
    DateTimeOffset CreatedAtUtc);

internal sealed record GreetingHistoryApiResponse(IReadOnlyCollection<GreetingApiResponse> Greetings);
