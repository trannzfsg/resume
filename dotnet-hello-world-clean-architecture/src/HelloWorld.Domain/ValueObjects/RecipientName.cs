using HelloWorld.Domain.Common;
using HelloWorld.Domain.Errors;

namespace HelloWorld.Domain.ValueObjects;

public sealed record RecipientName
{
    private const int MaxLength = 80;

    private RecipientName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<RecipientName> Create(string? value)
    {
        var trimmedValue = value?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedValue))
        {
            return Result.Failure<RecipientName>(DomainErrors.RecipientName.Empty);
        }

        if (trimmedValue.Length > MaxLength)
        {
            return Result.Failure<RecipientName>(DomainErrors.RecipientName.TooLong);
        }

        if (trimmedValue.Any(char.IsControl))
        {
            return Result.Failure<RecipientName>(DomainErrors.RecipientName.ContainsControlCharacters);
        }

        return Result.Success(new RecipientName(trimmedValue));
    }

    public override string ToString() => Value;
}
