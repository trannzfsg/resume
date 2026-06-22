using HelloWorld.Domain.Common;
using HelloWorld.Domain.Errors;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.Domain.Entities;

public sealed class Greeting
{
    private Greeting(
        Guid id,
        RecipientName recipient,
        GreetingMessage message,
        DateTimeOffset createdAtUtc)
    {
        Id = id;
        Recipient = recipient;
        Message = message;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; }

    public RecipientName Recipient { get; }

    public GreetingMessage Message { get; }

    public DateTimeOffset CreatedAtUtc { get; }

    public static Result<Greeting> Create(
        Guid id,
        RecipientName recipient,
        GreetingMessage message,
        DateTimeOffset createdAtUtc)
    {
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(message);

        if (id == Guid.Empty)
        {
            return Result.Failure<Greeting>(DomainErrors.Greeting.EmptyId);
        }

        if (createdAtUtc.Offset != TimeSpan.Zero)
        {
            return Result.Failure<Greeting>(DomainErrors.Greeting.NonUtcTimestamp);
        }

        return Result.Success(new Greeting(id, recipient, message, createdAtUtc));
    }
}
