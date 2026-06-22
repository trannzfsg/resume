using HelloWorld.Domain.Common;

namespace HelloWorld.Application.Abstractions;

public interface IRequestValidator<in TRequest>
{
    Result Validate(TRequest request);
}
