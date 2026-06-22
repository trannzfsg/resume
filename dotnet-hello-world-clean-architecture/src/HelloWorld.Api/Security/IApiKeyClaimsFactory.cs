using System.Security.Claims;

namespace HelloWorld.Api.Security;

public interface IApiKeyClaimsFactory
{
    ClaimsPrincipal CreatePrincipal(ApiKeyAuthenticationOptions options);
}
