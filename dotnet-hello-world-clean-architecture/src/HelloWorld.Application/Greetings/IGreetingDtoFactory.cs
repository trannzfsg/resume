using HelloWorld.Domain.Entities;

namespace HelloWorld.Application.Greetings;

public interface IGreetingDtoFactory
{
    GreetingDto Create(Greeting greeting);

    IReadOnlyCollection<GreetingDto> CreateMany(IEnumerable<Greeting> greetings);
}
