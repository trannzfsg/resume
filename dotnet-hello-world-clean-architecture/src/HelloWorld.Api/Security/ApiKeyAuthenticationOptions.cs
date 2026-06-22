using Microsoft.AspNetCore.Authentication;

namespace HelloWorld.Api.Security;

public sealed class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = ApiKeyAuthenticationDefaults.DefaultHeaderName;

    public string ApiKey { get; set; } = string.Empty;

    public string ClientName { get; set; } = "HelloWorld API client";

    public string[] Scopes { get; set; } = [];
}
