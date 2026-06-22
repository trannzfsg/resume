namespace HelloWorld.Api.Models;

public sealed record GreetingHistoryResponse(IReadOnlyCollection<GreetingResponse> Greetings);
