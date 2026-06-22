using HelloWorld.Domain.Common;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.Domain.Services;

public interface IGreetingComposer
{
    Result<GreetingMessage> Compose(RecipientName recipient);
}
