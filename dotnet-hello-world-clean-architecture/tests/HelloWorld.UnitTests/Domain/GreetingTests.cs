using HelloWorld.Domain.Entities;
using HelloWorld.Domain.Errors;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.UnitTests.Domain;

public sealed class GreetingTests
{
    private static readonly Guid GreetingId = Guid.Parse("43b5d25d-8772-481c-b3cb-104755448665");
    private static readonly DateTimeOffset CreatedAtUtc = new(2026, 6, 20, 1, 2, 3, TimeSpan.Zero);

    [Fact]
    public void Create_accepts_valid_greeting()
    {
        var recipient = RecipientName.Create("Tran").Value;
        var message = GreetingMessage.Create("Hello, Tran!").Value;

        var result = Greeting.Create(GreetingId, recipient, message, CreatedAtUtc);

        Assert.True(result.IsSuccess);
        Assert.Equal(GreetingId, result.Value.Id);
        Assert.Equal(recipient, result.Value.Recipient);
        Assert.Equal(message, result.Value.Message);
        Assert.Equal(CreatedAtUtc, result.Value.CreatedAtUtc);
    }

    [Fact]
    public void Create_rejects_empty_id()
    {
        var recipient = RecipientName.Create("Tran").Value;
        var message = GreetingMessage.Create("Hello, Tran!").Value;

        var result = Greeting.Create(Guid.Empty, recipient, message, CreatedAtUtc);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Greeting.EmptyId, result.Error);
    }

    [Fact]
    public void Create_rejects_non_utc_timestamp()
    {
        var recipient = RecipientName.Create("Tran").Value;
        var message = GreetingMessage.Create("Hello, Tran!").Value;
        var nonUtcTimestamp = new DateTimeOffset(2026, 6, 20, 11, 2, 3, TimeSpan.FromHours(10));

        var result = Greeting.Create(GreetingId, recipient, message, nonUtcTimestamp);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Greeting.NonUtcTimestamp, result.Error);
    }
}
