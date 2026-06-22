using HelloWorld.Domain.Services;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.UnitTests.Domain;

public sealed class FriendlyGreetingComposerTests
{
    [Fact]
    public void Compose_creates_hello_message_for_recipient()
    {
        var recipient = RecipientName.Create("Tran").Value;
        var composer = new FriendlyGreetingComposer();

        var result = composer.Compose(recipient);

        Assert.True(result.IsSuccess);
        Assert.Equal("Hello, Tran!", result.Value.Value);
    }
}
