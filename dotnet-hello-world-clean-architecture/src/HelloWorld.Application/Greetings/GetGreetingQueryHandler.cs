using HelloWorld.Application.Abstractions;
using HelloWorld.Domain.Common;
using HelloWorld.Domain.Entities;
using HelloWorld.Domain.Services;
using HelloWorld.Domain.ValueObjects;

namespace HelloWorld.Application.Greetings;

public sealed class GetGreetingQueryHandler : IGetGreetingQueryHandler
{
    private readonly IRequestValidator<GetGreetingQuery> _validator;
    private readonly IGreetingComposer _composer;
    private readonly IGreetingWriter _greetingWriter;
    private readonly IGreetingDtoFactory _greetingDtoFactory;
    private readonly IClock _clock;
    private readonly IIdGenerator _idGenerator;

    public GetGreetingQueryHandler(
        IRequestValidator<GetGreetingQuery> validator,
        IGreetingComposer composer,
        IGreetingWriter greetingWriter,
        IGreetingDtoFactory greetingDtoFactory,
        IClock clock,
        IIdGenerator idGenerator)
    {
        _validator = validator;
        _composer = composer;
        _greetingWriter = greetingWriter;
        _greetingDtoFactory = greetingDtoFactory;
        _clock = clock;
        _idGenerator = idGenerator;
    }

    public async Task<Result<GreetingDto>> HandleAsync(
        GetGreetingQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var validation = _validator.Validate(query);
        if (validation.IsFailure)
        {
            return Result.Failure<GreetingDto>(validation.Error);
        }

        var recipient = RecipientName.Create(query.NameOrDefault);
        if (recipient.IsFailure)
        {
            return Result.Failure<GreetingDto>(recipient.Error);
        }

        var message = _composer.Compose(recipient.Value);
        if (message.IsFailure)
        {
            return Result.Failure<GreetingDto>(message.Error);
        }

        var greeting = Greeting.Create(
            _idGenerator.NewId(),
            recipient.Value,
            message.Value,
            _clock.UtcNow);

        if (greeting.IsFailure)
        {
            return Result.Failure<GreetingDto>(greeting.Error);
        }

        await _greetingWriter.SaveAsync(greeting.Value, cancellationToken);

        return Result.Success(_greetingDtoFactory.Create(greeting.Value));
    }
}
