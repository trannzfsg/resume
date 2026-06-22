using HelloWorld.Domain.Entities;

namespace HelloWorld.Application.Abstractions;

public interface IGreetingReader
{
    Task<IReadOnlyCollection<Greeting>> ListRecentAsync(
        int maximumCount,
        CancellationToken cancellationToken);
}
