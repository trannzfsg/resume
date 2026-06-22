namespace HelloWorld.Application.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
