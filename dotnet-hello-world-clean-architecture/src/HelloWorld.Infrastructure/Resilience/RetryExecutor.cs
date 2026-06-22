using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HelloWorld.Infrastructure.Resilience;

public sealed class RetryExecutor
{
    private readonly ILogger<RetryExecutor> _logger;
    private readonly RetryOptions _options;

    public RetryExecutor(
        IOptions<RetryOptions> options,
        ILogger<RetryExecutor> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task ExecuteAsync(
        string operationName,
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken)
    {
        await ExecuteAsync(
            operationName,
            async token =>
            {
                await operation(token);
                return true;
            },
            cancellationToken);
    }

    public async Task<T> ExecuteAsync<T>(
        string operationName,
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken)
    {
        var maxAttempts = Math.Max(1, _options.MaxAttempts);
        var baseDelay = Math.Max(0, _options.BaseDelayMilliseconds);

        for (var attempt = 1; ; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                return await operation(cancellationToken);
            }
            catch (Exception exception) when (ShouldRetry(exception, cancellationToken) && attempt < maxAttempts)
            {
                var delay = TimeSpan.FromMilliseconds(baseDelay * Math.Pow(2, attempt - 1));

                _logger.LogWarning(
                    exception,
                    "Transient failure during {OperationName}. Retrying attempt {NextAttempt}/{MaxAttempts} after {DelayMilliseconds}ms.",
                    operationName,
                    attempt + 1,
                    maxAttempts,
                    delay.TotalMilliseconds);

                await Task.Delay(delay, cancellationToken);
            }
        }
    }

    private static bool ShouldRetry(Exception exception, CancellationToken cancellationToken) =>
        !cancellationToken.IsCancellationRequested
        && exception is TimeoutException or IOException or HttpRequestException;
}
