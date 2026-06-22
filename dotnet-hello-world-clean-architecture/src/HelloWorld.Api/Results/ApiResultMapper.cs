using HelloWorld.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Api.Results;

public sealed class ApiResultMapper : IApiResultMapper
{
    public ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        ControllerBase controller,
        Result<TValue> result,
        Func<TValue, TResponse> map,
        string validationFieldName)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(map);

        return result.IsSuccess
            ? controller.Ok(map(result.Value))
            : ToValidationProblem(controller, result.Error, validationFieldName);
    }

    private static ActionResult ToValidationProblem(
        ControllerBase controller,
        Error error,
        string fieldName) =>
        controller.ValidationProblem(new ValidationProblemDetails(
            new Dictionary<string, string[]>
            {
                [fieldName] = new[] { error.Message }
            })
        {
            Title = "Request validation failed.",
            Detail = error.Code,
            Status = StatusCodes.Status400BadRequest
        });
}
