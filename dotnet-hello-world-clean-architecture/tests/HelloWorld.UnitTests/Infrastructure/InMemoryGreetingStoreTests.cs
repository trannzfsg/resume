using HelloWorld.Domain.Entities;
using HelloWorld.Domain.ValueObjects;
using HelloWorld.Infrastructure.GreetingHistory;

namespace HelloWorld.UnitTests.Infrastructure;

public sealed class InMemoryGreetingStoreTests
{
    [Fact]
    public async Task ListRecentAsync_returns_most_recent_greetings_first_and_respects_limit()
    {
        var store = new InMemoryGreetingStore();
        await store.SaveAsync(CreateGreeting("Tran", sequence: 1), CancellationToken.None);
        await store.SaveAsync(CreateGreeting("Alex", sequence: 2), CancellationToken.None);
        await store.SaveAsync(CreateGreeting("Morgan", sequence: 3), CancellationToken.None);

        var greetings = await store.ListRecentAsync(2, CancellationToken.None);

        Assert.Collection(
            greetings,
            greeting => Assert.Equal("Morgan", greeting.Recipient.Value),
            greeting => Assert.Equal("Alex", greeting.Recipient.Value));
    }

    [Fact]
    public async Task SaveAsync_trims_oldest_greetings_when_store_reaches_capacity()
    {
        var store = new InMemoryGreetingStore();

        for (var index = 1; index <= 105; index++)
        {
            await store.SaveAsync(CreateGreeting($"Recipient {index}", index), CancellationToken.None);
        }

        var greetings = await store.ListRecentAsync(200, CancellationToken.None);

        Assert.Equal(100, greetings.Count);
        Assert.Equal("Recipient 105", greetings.First().Recipient.Value);
        Assert.Equal("Recipient 6", greetings.Last().Recipient.Value);
    }

    [Fact]
    public async Task SaveAsync_throws_when_cancelled_without_storing()
    {
        var store = new InMemoryGreetingStore();
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            store.SaveAsync(CreateGreeting("Tran", sequence: 1), cancellationTokenSource.Token));

        var greetings = await store.ListRecentAsync(10, CancellationToken.None);
        Assert.Empty(greetings);
    }

    [Fact]
    public async Task ListRecentAsync_throws_when_cancelled()
    {
        var store = new InMemoryGreetingStore();
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            store.ListRecentAsync(10, cancellationTokenSource.Token));
    }

    private static Greeting CreateGreeting(string recipientName, int sequence)
    {
        var recipient = RecipientName.Create(recipientName).Value;
        var message = GreetingMessage.Create($"Hello, {recipientName}!").Value;

        return Greeting.Create(
            Guid.Parse($"43b5d25d-8772-481c-b3cb-{sequence:000000000000}"),
            recipient,
            message,
            new DateTimeOffset(2026, 6, 20, 1, sequence % 60, 0, TimeSpan.Zero)).Value;
    }
}
