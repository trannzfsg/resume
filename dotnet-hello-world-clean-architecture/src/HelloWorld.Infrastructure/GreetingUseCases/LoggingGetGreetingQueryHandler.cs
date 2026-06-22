using System.Diagnostics;
using HelloWorld.Application.Greetings;
using HelloWorld.Domain.Common;
using HelloWorld.Infrastructure.Observability;
using Microsoft.Extensions.Logging;

namespace HelloWorld.Infrastructure.GreetingUseCases;

public sealed class LoggingGetGreetingQueryHandler : IGetGreetingQueryHandler
{
    private readonly IGetGreetingQueryHandler _inner;
    private readonly ILogger<LoggingGetGreetingQueryHandler> _logger;

    public LoggingGetGreetingQueryHandler(
        IGetGreetingQueryHandler inner,
        ILogger<LoggingGetGreetingQueryHandler> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<GreetingDto>> HandleAsync(
        GetGreetingQuery query,
        CancellationToken cancellationToken)
    {
        using var activity = GreetingDiagnostics.ActivitySource.StartActivity(
            "Greeting.Get",
            ActivityKind.Internal);

        activity?.SetTag("greeting.recipient.requested", query.NameOrDefault);

        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["RecipientName"] = query.NameOrDefault
        });

        _logger.LogInformation("Handling greeting request.");

        var result = await _inner.HandleAsync(query, cancellationToken);

        if (result.IsSuccess)
        {
            activity?.SetStatus(ActivityStatusCode.Ok);
            activity?.SetTag("greeting.id", result.Value.Id);
            activity?.SetTag("greeting.recipient", result.Value.Recipient);

            GreetingDiagnostics.GreetingsCreated.Add(
                1,
                new KeyValuePair<string, object?>("greeting.recipient", result.Value.Recipient));

            _logger.LogInformation(
                "Greeting {GreetingId} created for {RecipientName}.",
                result.Value.Id,
                result.Value.Recipient);
        }
        else
        {
            activity?.SetStatus(ActivityStatusCode.Error, result.Error.Code);
            activity?.SetTag("error.code", result.Error.Code);
            activity?.SetTag("error.message", result.Error.Message);

            GreetingDiagnostics.GreetingRequestsRejected.Add(
                1,
                new KeyValuePair<string, object?>("error.code", result.Error.Code));

            _logger.LogWarning(
                "Greeting request rejected with {ErrorCode}: {ErrorMessage}",
                result.Error.Code,
                result.Error.Message);
        }

        return result;
    }
}
