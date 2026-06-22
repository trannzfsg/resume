using HelloWorld.Domain.Errors;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.UnitTests.Domain;

public sealed class GreetingMessageTests
{
    [Theory]
    [InlineData("Hello, Tran!")]
    [InlineData(" Hello, Tran! ")]
    public void Create_accepts_valid_message(string message)
    {
        var result = GreetingMessage.Create(message);

        Assert.True(result.IsSuccess);
        Assert.Equal(message, result.Value.Value);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(200)]
    public void Create_accepts_message_up_to_maximum_length(int length)
    {
        var message = new string('a', length);

        var result = GreetingMessage.Create(message);

        Assert.True(result.IsSuccess);
        Assert.Equal(message, result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_rejects_empty_message(string? input)
    {
        var result = GreetingMessage.Create(input);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.GreetingMessage.Empty, result.Error);
    }

    [Theory]
    [InlineData(201)]
    [InlineData(250)]
    public void Create_rejects_message_above_maximum_length(int length)
    {
        var result = GreetingMessage.Create(new string('a', length));

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.GreetingMessage.TooLong, result.Error);
    }
}
