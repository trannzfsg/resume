namespace HelloWorld.Application.Greetings;

public sealed record ListGreetingsQuery(int MaximumCount)
{
    public const int MinimumCount = 1;
    public const int DefaultCount = 10;
    public const int MaximumAllowedCount = 50;
}
