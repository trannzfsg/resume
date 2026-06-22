namespace HelloWorld.Application.Greetings;

public sealed record GetGreetingQuery(string? Name)
{
    public const string DefaultName = "World";

    public string NameOrDefault => Name ?? DefaultName;
}
