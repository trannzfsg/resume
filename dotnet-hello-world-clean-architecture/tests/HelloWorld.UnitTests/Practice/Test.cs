namespace HelloWorld.UnitTests.Practice;

public record TestRecord
{
    public string? A { get; init; }
    public string? B { get; init; }
}

public record TestRecord2
{
    public TestRecord2(string? a, string? b)
    {
        A = a;
        B = b;
    }
    public string? A { get; }
    public string? B { get; }
}

public record TestRecord3 (string? a, string? b);


public class TestClass
{
    public bool DoSomething()
    {
        var test = new TestRecord { A = "a", B = "b" };
        var test2 = new TestRecord3 ("a", "b");
        var test3 = test2 with { a = "a" };

        return test2.Equals(test3);
    }

    public IReadOnlyList<string> TopWordsStartWithA(string? text, int count)
    {
        return (text ?? "").ToLowerInvariant()
                           .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                           .Where(valid => valid.StartsWith('a'))
                           .GroupBy(group => group)
                           .OrderByDescending(order => order.Count())
                           .ThenBy(group => group.Key)
                           .Take(count)
                           .Select(group => group.Key)
                           .ToArray();
                           
    }
}
