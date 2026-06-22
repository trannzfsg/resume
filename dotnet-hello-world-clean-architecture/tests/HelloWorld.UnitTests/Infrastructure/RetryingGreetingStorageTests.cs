using HelloWorld.Application.Abstractions;
using HelloWorld.Domain.Entities;
using HelloWorld.Domain.ValueObjects;
using HelloWorld.Infrastructure.Resilience;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace HelloWorld.UnitTests.Infrastructure;

public sealed class RetryingGreetingStorageTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    public async Task SaveAsync_retries_transient_failures_then_succeeds(
        int failuresBeforeSuccess,
        int expectedAttempts)
    {
        var inner = new FlakyGreetingWriter(failuresBeforeSuccess);
        var writer = CreateWriter(inner);

        await writer.SaveAsync(CreateGreeting(), CancellationToken.None);

        Assert.Equal(expectedAttempts, inner.SaveAttempts);
    }

    [Fact]
    public async Task SaveAsync_does_not_retry_non_transient_failures()
    {
        var inner = new ThrowingGreetingWriter(new InvalidOperationException("Not transient."));
        var writer = CreateWriter(inner);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            writer.SaveAsync(CreateGreeting(), CancellationToken.None));

        Assert.Equal(1, inner.SaveAttempts);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task ListRecentAsync_stops_after_configured_retry_limit(int maxAttempts)
    {
        var inner = new ThrowingGreetingReader(new TimeoutException("Still unavailable."));
        var reader = CreateReader(inner, maxAttempts);

        await Assert.ThrowsAsync<TimeoutException>(() =>
            reader.ListRecentAsync(10, CancellationToken.None));

        Assert.Equal(maxAttempts, inner.ListAttempts);
    }

    [Fact]
    public async Task SaveAsync_does_not_call_inner_writer_when_cancelled_before_first_attempt()
    {
        var inner = new FlakyGreetingWriter(failuresBeforeSuccess: 0);
        var writer = CreateWriter(inner);
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            writer.SaveAsync(CreateGreeting(), cancellationTokenSource.Token));

        Assert.Equal(0, inner.SaveAttempts);
    }

    private static RetryingGreetingWriter CreateWriter(
        IGreetingWriter inner,
        int maxAttempts = 3) =>
        new(
            inner,
            CreateRetryExecutor(maxAttempts));

    private static RetryingGreetingReader CreateReader(
        IGreetingReader inner,
        int maxAttempts = 3) =>
        new(
            inner,
            CreateRetryExecutor(maxAttempts));

    private static RetryExecutor CreateRetryExecutor(int maxAttempts) =>
        new(
            Options.Create(new RetryOptions
            {
                MaxAttempts = maxAttempts,
                BaseDelayMilliseconds = 0
            }),
            NullLogger<RetryExecutor>.Instance);

    private static Greeting CreateGreeting()
    {
        var recipient = RecipientName.Create("Tran").Value;
        var message = GreetingMessage.Create("Hello, Tran!").Value;

        return Greeting.Create(
            Guid.Parse("43b5d25d-8772-481c-b3cb-104755448665"),
            recipient,
            message,
            new DateTimeOffset(2026, 6, 20, 1, 2, 3, TimeSpan.Zero)).Value;
    }

    private sealed class FlakyGreetingWriter : IGreetingWriter
    {
        private readonly int _failuresBeforeSuccess;

        public FlakyGreetingWriter(int failuresBeforeSuccess)
        {
            _failuresBeforeSuccess = failuresBeforeSuccess;
        }

        public int SaveAttempts { get; private set; }

        public Task SaveAsync(Greeting greeting, CancellationToken cancellationToken)
        {
            SaveAttempts++;

            if (SaveAttempts <= _failuresBeforeSuccess)
            {
                throw new TimeoutException("Temporarily unavailable.");
            }

            return Task.CompletedTask;
        }
    }

    private sealed class ThrowingGreetingWriter : IGreetingWriter
    {
        private readonly Exception _exception;

        public ThrowingGreetingWriter(Exception exception)
        {
            _exception = exception;
        }

        public int SaveAttempts { get; private set; }

        public Task SaveAsync(Greeting greeting, CancellationToken cancellationToken)
        {
            SaveAttempts++;
            throw _exception;
        }
    }

    private sealed class ThrowingGreetingReader : IGreetingReader
    {
        private readonly Exception _exception;

        public ThrowingGreetingReader(Exception exception)
        {
            _exception = exception;
        }

        public int ListAttempts { get; private set; }

        public Task<IReadOnlyCollection<Greeting>> ListRecentAsync(
            int maximumCount,
            CancellationToken cancellationToken)
        {
            ListAttempts++;
            throw _exception;
        }
    }
}
