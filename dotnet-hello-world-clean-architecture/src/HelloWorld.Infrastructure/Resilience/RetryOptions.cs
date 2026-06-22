namespace HelloWorld.Infrastructure.Resilience;

public sealed class RetryOptions
{
    public int MaxAttempts { get; set; } = 3;

    public int BaseDelayMilliseconds { get; set; } = 100;
}
