using HelloWorld.Domain.Common;
using HelloWorld.Domain.Errors;

namespace HelloWorld.Domain.ValueObjects;

public sealed record GreetingMessage
{
    private const int MaxLength = 200;

    private GreetingMessage(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<GreetingMessage> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<GreetingMessage>(DomainErrors.GreetingMessage.Empty);
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<GreetingMessage>(DomainErrors.GreetingMessage.TooLong);
        }

        return Result.Success(new GreetingMessage(value));
    }

    public override string ToString() => Value;
}
