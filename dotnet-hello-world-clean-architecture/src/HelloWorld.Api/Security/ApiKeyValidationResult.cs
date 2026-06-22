namespace HelloWorld.Api.Security;

public sealed record ApiKeyValidationResult(bool IsSuccess, string FailureMessage)
{
    public static ApiKeyValidationResult Success() => new(true, string.Empty);

    public static ApiKeyValidationResult Failure(string failureMessage) => new(false, failureMessage);
}
