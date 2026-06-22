using HelloWorld.Application.Abstractions;
using HelloWorld.Application.Common;
using HelloWorld.Domain.Common;

namespace HelloWorld.Application.Greetings;

public sealed class ListGreetingsQueryValidator : IRequestValidator<ListGreetingsQuery>
{
    public Result Validate(ListGreetingsQuery request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return request.MaximumCount is >= ListGreetingsQuery.MinimumCount and <= ListGreetingsQuery.MaximumAllowedCount
            ? Result.Success()
            : Result.Failure(ApplicationErrors.InvalidRecentGreetingLimit(
                ListGreetingsQuery.MinimumCount,
                ListGreetingsQuery.MaximumAllowedCount));
    }
}
