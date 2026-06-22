using HelloWorld.Application.Abstractions;
using HelloWorld.Domain.Common;

namespace HelloWorld.Application.Greetings;

public sealed class ListGreetingsQueryHandler : IListGreetingsQueryHandler
{
    private readonly IRequestValidator<ListGreetingsQuery> _validator;
    private readonly IGreetingReader _greetingReader;
    private readonly IGreetingDtoFactory _greetingDtoFactory;

    public ListGreetingsQueryHandler(
        IRequestValidator<ListGreetingsQuery> validator,
        IGreetingReader greetingReader,
        IGreetingDtoFactory greetingDtoFactory)
    {
        _validator = validator;
        _greetingReader = greetingReader;
        _greetingDtoFactory = greetingDtoFactory;
    }

    public async Task<Result<IReadOnlyCollection<GreetingDto>>> HandleAsync(
        ListGreetingsQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var validation = _validator.Validate(query);
        if (validation.IsFailure)
        {
            return Result.Failure<IReadOnlyCollection<GreetingDto>>(validation.Error);
        }

        var greetings = await _greetingReader.ListRecentAsync(query.MaximumCount, cancellationToken);
        var greetingDtos = _greetingDtoFactory.CreateMany(greetings);

        return Result.Success<IReadOnlyCollection<GreetingDto>>(greetingDtos);
    }
}
