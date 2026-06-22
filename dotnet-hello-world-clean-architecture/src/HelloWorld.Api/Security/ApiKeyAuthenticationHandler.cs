using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace HelloWorld.Api.Security;

public sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IApiKeyClaimsFactory _claimsFactory;
    private readonly IApiKeyValidator _apiKeyValidator;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IApiKeyValidator apiKeyValidator,
        IApiKeyClaimsFactory claimsFactory)
        : base(options, logger, encoder)
    {
        _apiKeyValidator = apiKeyValidator;
        _claimsFactory = claimsFactory;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(Options.HeaderName, out var submittedApiKeys))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var validation = _apiKeyValidator.Validate(submittedApiKeys, Options);
        if (!validation.IsSuccess)
        {
            return Task.FromResult(AuthenticateResult.Fail(validation.FailureMessage));
        }

        var principal = _claimsFactory.CreatePrincipal(Options);
        var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationDefaults.AuthenticationScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.WWWAuthenticate = ApiKeyAuthenticationDefaults.AuthenticationScheme;
        return base.HandleChallengeAsync(properties);
    }

}
