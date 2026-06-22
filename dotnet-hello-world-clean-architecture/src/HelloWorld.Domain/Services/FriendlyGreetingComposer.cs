using HelloWorld.Domain.Common;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.Domain.Services;

public sealed class FriendlyGreetingComposer : IGreetingComposer
{
    public Result<GreetingMessage> Compose(RecipientName recipient)
    {
        ArgumentNullException.ThrowIfNull(recipient);

        return GreetingMessage.Create($"Hello, {recipient.Value}!");
    }
}
