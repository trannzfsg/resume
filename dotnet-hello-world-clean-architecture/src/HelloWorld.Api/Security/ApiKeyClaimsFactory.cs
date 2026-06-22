using System.Security.Claims;

namespace HelloWorld.Api.Security;

public sealed class ApiKeyClaimsFactory : IApiKeyClaimsFactory
{
    public ClaimsPrincipal CreatePrincipal(ApiKeyAuthenticationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, options.ClientName),
            new(ClaimTypes.Name, options.ClientName)
        };

        claims.AddRange(options.Scopes.Select(scope =>
            new Claim(ApiKeyAuthenticationDefaults.ScopeClaimType, scope)));

        var identity = new ClaimsIdentity(
            claims,
            ApiKeyAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name,
            ApiKeyAuthenticationDefaults.ScopeClaimType);

        return new ClaimsPrincipal(identity);
    }
}
