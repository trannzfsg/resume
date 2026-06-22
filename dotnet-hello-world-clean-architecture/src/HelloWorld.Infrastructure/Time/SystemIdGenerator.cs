using HelloWorld.Application.Abstractions;

namespace HelloWorld.Infrastructure.Time;

public sealed class SystemIdGenerator : IIdGenerator
{
    public Guid NewId() => Guid.NewGuid();
}
