using HelloWorld.Domain.Errors;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.UnitTests.Domain;

public sealed class RecipientNameTests
{
    [Theory]
    [InlineData("Tran", "Tran")]
    [InlineData("  Tran  ", "Tran")]
    [InlineData("\tTran\t", "Tran")]
    public void Create_trims_valid_recipient_name(string input, string expectedValue)
    {
        var result = RecipientName.Create(input);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedValue, result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_rejects_empty_recipient_name(string? input)
    {
        var result = RecipientName.Create(input);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.RecipientName.Empty, result.Error);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(80)]
    public void Create_accepts_recipient_name_up_to_maximum_length(int length)
    {
        var value = new string('a', length);

        var result = RecipientName.Create(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Theory]
    [InlineData(81)]
    [InlineData(100)]
    public void Create_rejects_recipient_name_above_maximum_length(int length)
    {
        var result = RecipientName.Create(new string('a', length));

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.RecipientName.TooLong, result.Error);
    }

    [Theory]
    [InlineData("Tran\r\nZha")]
    [InlineData("Tran\u001fZha")]
    public void Create_rejects_control_characters(string input)
    {
        var result = RecipientName.Create(input);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.RecipientName.ContainsControlCharacters, result.Error);
    }
}
