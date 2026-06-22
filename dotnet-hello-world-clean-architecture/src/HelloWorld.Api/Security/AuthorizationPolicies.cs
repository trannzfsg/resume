namespace HelloWorld.Api.Security;

public static class AuthorizationPolicies
{
    public const string GreetingRead = "GreetingRead";
    public const string GreetingHistoryRead = "GreetingHistoryRead";

    public const string GreetingReadScope = "greetings:read";
    public const string GreetingHistoryReadScope = "greetings:history";
}
