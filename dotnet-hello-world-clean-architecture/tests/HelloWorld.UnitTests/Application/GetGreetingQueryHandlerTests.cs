using HelloWorld.Application.Abstractions;
using HelloWorld.Application.Greetings;
using HelloWorld.Domain.Entities;
using HelloWorld.Domain.Services;

namespace HelloWorld.UnitTests.Application;

public sealed class GetGreetingQueryHandlerTests
{
    private static readonly Guid GreetingId = Guid.Parse("43b5d25d-8772-481c-b3cb-104755448665");
    private static readonly DateTimeOffset Now = new(2026, 6, 19, 1, 2, 3, TimeSpan.Zero);

    [Fact]
    public async Task HandleAsync_creates_greeting_and_saves_it()
    {
        var greetingWriter = new RecordingGreetingWriter();
        var handler = CreateHandler(greetingWriter);

        var result = await handler.HandleAsync(new GetGreetingQuery("Tran"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(GreetingId, result.Value.Id);
        Assert.Equal("Tran", result.Value.Recipient);
        Assert.Equal("Hello, Tran!", result.Value.Message);
        Assert.Equal(Now, result.Value.CreatedAtUtc);

        var savedGreeting = Assert.Single(greetingWriter.SavedGreetings);
        Assert.Equal(GreetingId, savedGreeting.Id);
        Assert.Equal("Tran", savedGreeting.Recipient.Value);
        Assert.Equal("Hello, Tran!", savedGreeting.Message.Value);
        Assert.Equal(Now, savedGreeting.CreatedAtUtc);
    }

    [Fact]
    public async Task HandleAsync_defaults_to_world_when_name_is_omitted()
    {
        var greetingWriter = new RecordingGreetingWriter();
        var handler = CreateHandler(greetingWriter);

        var result = await handler.HandleAsync(new GetGreetingQuery(null), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("World", result.Value.Recipient);
        Assert.Equal("Hello, World!", result.Value.Message);
    }

    [Fact]
    public async Task HandleAsync_rejects_invalid_name_without_saving()
    {
        var greetingWriter = new RecordingGreetingWriter();
        var handler = CreateHandler(greetingWriter);

        var result = await handler.HandleAsync(new GetGreetingQuery("Tran\r\nZha"), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Empty(greetingWriter.SavedGreetings);
    }

    private static GetGreetingQueryHandler CreateHandler(IGreetingWriter greetingWriter) =>
        new(
            new GetGreetingQueryValidator(),
            new FriendlyGreetingComposer(),
            greetingWriter,
            new GreetingDtoFactory(),
            new FixedClock(Now),
            new FixedIdGenerator(GreetingId));

    private sealed class FixedClock : IClock
    {
        private readonly DateTimeOffset _utcNow;

        public FixedClock(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public DateTimeOffset UtcNow => _utcNow;
    }

    private sealed class FixedIdGenerator : IIdGenerator
    {
        private readonly Guid _id;

        public FixedIdGenerator(Guid id)
        {
            _id = id;
        }

        public Guid NewId() => _id;
    }

    private sealed class RecordingGreetingWriter : IGreetingWriter
    {
        private readonly List<Greeting> _savedGreetings = new();

        public IReadOnlyCollection<Greeting> SavedGreetings => _savedGreetings;

        public Task SaveAsync(Greeting greeting, CancellationToken cancellationToken)
        {
            _savedGreetings.Add(greeting);
            return Task.CompletedTask;
        }
    }
}
