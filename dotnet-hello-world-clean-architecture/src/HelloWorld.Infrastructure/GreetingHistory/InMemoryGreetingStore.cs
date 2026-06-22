using System.Collections.Concurrent;
using HelloWorld.Application.Abstractions;
using HelloWorld.Domain.Entities;

namespace HelloWorld.Infrastructure.GreetingHistory;

public sealed class InMemoryGreetingStore : IGreetingReader, IGreetingWriter
{
    private const int MaximumStoredGreetings = 100;

    private readonly ConcurrentQueue<Greeting> _greetings = new();

    public Task SaveAsync(Greeting greeting, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(greeting);
        cancellationToken.ThrowIfCancellationRequested();

        _greetings.Enqueue(greeting);

        while (_greetings.Count > MaximumStoredGreetings)
        {
            _greetings.TryDequeue(out _);
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Greeting>> ListRecentAsync(
        int maximumCount,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<Greeting> recentGreetings = _greetings
            .ToArray()
            .Reverse()
            .Take(maximumCount)
            .ToArray();

        return Task.FromResult(recentGreetings);
    }
}
