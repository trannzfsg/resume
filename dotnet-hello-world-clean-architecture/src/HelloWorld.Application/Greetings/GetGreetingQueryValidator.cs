using HelloWorld.Application.Abstractions;
using HelloWorld.Domain.Common;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.Application.Greetings;

public sealed class GetGreetingQueryValidator : IRequestValidator<GetGreetingQuery>
{
    public Result Validate(GetGreetingQuery request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var recipientName = RecipientName.Create(request.NameOrDefault);

        return recipientName.IsSuccess
            ? Result.Success()
            : Result.Failure(recipientName.Error);
    }
}
