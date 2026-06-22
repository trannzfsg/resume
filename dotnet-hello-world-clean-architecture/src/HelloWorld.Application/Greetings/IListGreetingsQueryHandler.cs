using HelloWorld.Domain.Common;

namespace HelloWorld.Application.Greetings;

public interface IListGreetingsQueryHandler
{
    Task<Result<IReadOnlyCollection<GreetingDto>>> HandleAsync(
        ListGreetingsQuery query,
        CancellationToken cancellationToken);
}
