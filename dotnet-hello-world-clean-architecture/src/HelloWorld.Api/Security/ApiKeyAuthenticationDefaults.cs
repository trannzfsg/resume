namespace HelloWorld.Api.Security;

public static class ApiKeyAuthenticationDefaults
{
    public const string AuthenticationScheme = "ApiKey";
    public const string DefaultHeaderName = "X-Api-Key";
    public const string ScopeClaimType = "scope";
}
