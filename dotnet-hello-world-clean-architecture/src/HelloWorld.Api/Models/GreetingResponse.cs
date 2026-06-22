using HelloWorld.Application.Greetings;

namespace HelloWorld.Api.Models;

public sealed record GreetingResponse(
    Guid Id,
    string Recipient,
    string Message,
    DateTimeOffset CreatedAtUtc)
{
    public static GreetingResponse FromDto(GreetingDto greeting)
    {
        ArgumentNullException.ThrowIfNull(greeting);

        return new GreetingResponse(
            greeting.Id,
            greeting.Recipient,
            greeting.Message,
            greeting.CreatedAtUtc);
    }
}
