using HelloWorld.Domain.Common;

namespace HelloWorld.Application.Greetings;

public interface IGetGreetingQueryHandler
{
    Task<Result<GreetingDto>> HandleAsync(GetGreetingQuery query, CancellationToken cancellationToken);
}
