using HelloWorld.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Api.Results;

public interface IApiResultMapper
{
    ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        ControllerBase controller,
        Result<TValue> result,
        Func<TValue, TResponse> map,
        string validationFieldName);
}
