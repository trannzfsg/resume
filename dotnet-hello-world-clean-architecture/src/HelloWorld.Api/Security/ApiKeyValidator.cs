using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace HelloWorld.Api.Security;

public sealed class ApiKeyValidator : IApiKeyValidator
{
    public ApiKeyValidationResult Validate(
        StringValues submittedApiKeys,
        ApiKeyAuthenticationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (StringValues.IsNullOrEmpty(submittedApiKeys) || submittedApiKeys.Count != 1)
        {
            return ApiKeyValidationResult.Failure("Exactly one API key must be provided.");
        }

        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            return ApiKeyValidationResult.Failure("API key authentication is not configured.");
        }

        var submittedApiKey = submittedApiKeys[0];
        if (string.IsNullOrWhiteSpace(submittedApiKey) || !ApiKeysMatch(submittedApiKey, options.ApiKey))
        {
            return ApiKeyValidationResult.Failure("The API key is invalid.");
        }

        return ApiKeyValidationResult.Success();
    }

    private static bool ApiKeysMatch(string submittedApiKey, string configuredApiKey)
    {
        var submittedBytes = Encoding.UTF8.GetBytes(submittedApiKey);
        var configuredBytes = Encoding.UTF8.GetBytes(configuredApiKey);

        return submittedBytes.Length == configuredBytes.Length
            && CryptographicOperations.FixedTimeEquals(submittedBytes, configuredBytes);
    }
}
