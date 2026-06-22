using Microsoft.Extensions.Primitives;

namespace HelloWorld.Api.Security;

public interface IApiKeyValidator
{
    ApiKeyValidationResult Validate(
        StringValues submittedApiKeys,
        ApiKeyAuthenticationOptions options);
}
