using Microsoft.AspNetCore.Authorization;

namespace HelloWorld.Api.Security;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHelloWorldSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();
        services.AddSingleton<IApiKeyClaimsFactory, ApiKeyClaimsFactory>();

        services
            .AddAuthentication(ApiKeyAuthenticationDefaults.AuthenticationScheme)
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationDefaults.AuthenticationScheme,
                options => configuration.GetSection("Authentication:ApiKey").Bind(options));

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(AuthorizationPolicies.GreetingRead, policy =>
                policy.RequireClaim(
                    ApiKeyAuthenticationDefaults.ScopeClaimType,
                    AuthorizationPolicies.GreetingReadScope));

            options.AddPolicy(AuthorizationPolicies.GreetingHistoryRead, policy =>
                policy.RequireClaim(
                    ApiKeyAuthenticationDefaults.ScopeClaimType,
                    AuthorizationPolicies.GreetingHistoryReadScope));
        });

        return services;
    }
}
