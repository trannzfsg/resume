using HelloWorld.Application.Abstractions;
using HelloWorld.Domain.Entities;

namespace HelloWorld.Infrastructure.Resilience;

public sealed class RetryingGreetingReader : IGreetingReader
{
    private readonly IGreetingReader _inner;
    private readonly RetryExecutor _retryExecutor;

    public RetryingGreetingReader(
        IGreetingReader inner,
        RetryExecutor retryExecutor)
    {
        _inner = inner;
        _retryExecutor = retryExecutor;
    }

    public Task<IReadOnlyCollection<Greeting>> ListRecentAsync(
        int maximumCount,
        CancellationToken cancellationToken) =>
        _retryExecutor.ExecuteAsync(
            "ListRecentGreetings",
            token => _inner.ListRecentAsync(maximumCount, token),
            cancellationToken);
}
