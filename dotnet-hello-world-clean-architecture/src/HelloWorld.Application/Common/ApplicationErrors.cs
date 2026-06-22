using HelloWorld.Domain.Common;

namespace HelloWorld.Application.Common;

public static class ApplicationErrors
{
    public static Error InvalidRecentGreetingLimit(int minimum, int maximum) => new(
        "ListGreetings.InvalidLimit",
        $"The recent greeting limit must be between {minimum} and {maximum}.");
}
