using HelloWorld.Domain.Entities;

namespace HelloWorld.Application.Greetings;

public sealed class GreetingDtoFactory : IGreetingDtoFactory
{
    public GreetingDto Create(Greeting greeting)
    {
        ArgumentNullException.ThrowIfNull(greeting);

        return new GreetingDto(
            greeting.Id,
            greeting.Recipient.Value,
            greeting.Message.Value,
            greeting.CreatedAtUtc);
    }

    public IReadOnlyCollection<GreetingDto> CreateMany(IEnumerable<Greeting> greetings)
    {
        ArgumentNullException.ThrowIfNull(greetings);

        return greetings.Select(Create).ToArray();
    }
}
