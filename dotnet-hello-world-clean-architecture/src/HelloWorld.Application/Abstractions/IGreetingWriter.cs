using HelloWorld.Domain.Entities;

namespace HelloWorld.Application.Abstractions;

public interface IGreetingWriter
{
    Task SaveAsync(Greeting greeting, CancellationToken cancellationToken);
}
