using HelloWorld.Application.Abstractions;
using HelloWorld.Domain.Entities;

namespace HelloWorld.Infrastructure.Resilience;

public sealed class RetryingGreetingWriter : IGreetingWriter
{
    private readonly IGreetingWriter _inner;
    private readonly RetryExecutor _retryExecutor;

    public RetryingGreetingWriter(
        IGreetingWriter inner,
        RetryExecutor retryExecutor)
    {
        _inner = inner;
        _retryExecutor = retryExecutor;
    }

    public Task SaveAsync(Greeting greeting, CancellationToken cancellationToken) =>
        _retryExecutor.ExecuteAsync(
            "SaveGreeting",
            token => _inner.SaveAsync(greeting, token),
            cancellationToken);
}
