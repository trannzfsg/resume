namespace HelloWorld.Application.Greetings;

public sealed record GreetingDto(
    Guid Id,
    string Recipient,
    string Message,
    DateTimeOffset CreatedAtUtc);
