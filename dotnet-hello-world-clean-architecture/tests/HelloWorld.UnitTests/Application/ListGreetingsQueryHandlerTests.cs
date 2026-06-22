using HelloWorld.Application.Abstractions;
using HelloWorld.Application.Greetings;
using HelloWorld.Domain.Entities;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.UnitTests.Application;

public sealed class ListGreetingsQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_returns_recent_greetings_from_reader()
    {
        var greetings = new[]
        {
            CreateGreeting("Tran", minutes: 1),
            CreateGreeting("Alex", minutes: 2)
        };
        var greetingReader = new RecordingGreetingReader(greetings);
        var handler = new ListGreetingsQueryHandler(
            new ListGreetingsQueryValidator(),
            greetingReader,
            new GreetingDtoFactory());

        var result = await handler.HandleAsync(new ListGreetingsQuery(2), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, greetingReader.RequestedMaximumCount);
        Assert.Collection(
            result.Value,
            greeting => Assert.Equal("Tran", greeting.Recipient),
            greeting => Assert.Equal("Alex", greeting.Recipient));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(51)]
    [InlineData(100)]
    public async Task HandleAsync_rejects_limits_outside_allowed_range_without_reading(
        int requestedMaximum)
    {
        var greetingReader = new RecordingGreetingReader(Array.Empty<Greeting>());
        var handler = new ListGreetingsQueryHandler(
            new ListGreetingsQueryValidator(),
            greetingReader,
            new GreetingDtoFactory());

        var result = await handler.HandleAsync(new ListGreetingsQuery(requestedMaximum), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("ListGreetings.InvalidLimit", result.Error.Code);
        Assert.False(greetingReader.WasCalled);
    }

    private static Greeting CreateGreeting(string recipientName, int minutes)
    {
        var recipient = RecipientName.Create(recipientName).Value;
        var message = GreetingMessage.Create($"Hello, {recipientName}!").Value;

        return Greeting.Create(
            Guid.NewGuid(),
            recipient,
            message,
            new DateTimeOffset(2026, 6, 20, 1, minutes, 0, TimeSpan.Zero)).Value;
    }

    private sealed class RecordingGreetingReader : IGreetingReader
    {
        private readonly IReadOnlyCollection<Greeting> _greetings;

        public RecordingGreetingReader(IReadOnlyCollection<Greeting> greetings)
        {
            _greetings = greetings;
        }

        public bool WasCalled { get; private set; }

        public int? RequestedMaximumCount { get; private set; }

        public Task<IReadOnlyCollection<Greeting>> ListRecentAsync(
            int maximumCount,
            CancellationToken cancellationToken)
        {
            WasCalled = true;
            RequestedMaximumCount = maximumCount;
            return Task.FromResult(_greetings);
        }
    }
}
