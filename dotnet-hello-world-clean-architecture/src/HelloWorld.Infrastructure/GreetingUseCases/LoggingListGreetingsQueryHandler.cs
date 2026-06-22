using System.Diagnostics;
using HelloWorld.Application.Greetings;
using HelloWorld.Domain.Common;
using HelloWorld.Infrastructure.Observability;
using Microsoft.Extensions.Logging;

namespace HelloWorld.Infrastructure.GreetingUseCases;

public sealed class LoggingListGreetingsQueryHandler : IListGreetingsQueryHandler
{
    private readonly IListGreetingsQueryHandler _inner;
    private readonly ILogger<LoggingListGreetingsQueryHandler> _logger;

    public LoggingListGreetingsQueryHandler(
        IListGreetingsQueryHandler inner,
        ILogger<LoggingListGreetingsQueryHandler> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<GreetingDto>>> HandleAsync(
        ListGreetingsQuery query,
        CancellationToken cancellationToken)
    {
        using var activity = GreetingDiagnostics.ActivitySource.StartActivity(
            "Greeting.ListHistory",
            ActivityKind.Internal);

        activity?.SetTag("request.maximum_count", query.MaximumCount);

        _logger.LogInformation(
            "Listing up to {MaximumCount} recent greetings.",
            query.MaximumCount);

        var result = await _inner.HandleAsync(query, cancellationToken);

        if (result.IsSuccess)
        {
            activity?.SetStatus(ActivityStatusCode.Ok);
            activity?.SetTag("result.count", result.Value.Count);

            GreetingDiagnostics.GreetingHistoryRequests.Add(
                1,
                new KeyValuePair<string, object?>("result", "success"));

            _logger.LogInformation(
                "Returned {GreetingCount} recent greetings.",
                result.Value.Count);
        }
        else
        {
            activity?.SetStatus(ActivityStatusCode.Error, result.Error.Code);
            activity?.SetTag("error.code", result.Error.Code);
            activity?.SetTag("error.message", result.Error.Message);

            GreetingDiagnostics.GreetingHistoryRequests.Add(
                1,
                new KeyValuePair<string, object?>("result", "rejected"),
                new KeyValuePair<string, object?>("error.code", result.Error.Code));

            _logger.LogWarning(
                "Recent greeting request rejected with {ErrorCode}: {ErrorMessage}",
                result.Error.Code,
                result.Error.Message);
        }

        return result;
    }
}
