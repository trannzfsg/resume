using HelloWorld.Application.Abstractions;
using HelloWorld.Application.Greetings;
using HelloWorld.Domain.Services;

namespace HelloWorld.Api.Composition;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddHelloWorldApplication(this IServiceCollection services)
    {
        services.AddSingleton<IGreetingComposer, FriendlyGreetingComposer>();
        services.AddSingleton<IGreetingDtoFactory, GreetingDtoFactory>();
        services.AddSingleton<IRequestValidator<GetGreetingQuery>, GetGreetingQueryValidator>();
        services.AddSingleton<IRequestValidator<ListGreetingsQuery>, ListGreetingsQueryValidator>();

        services.AddScoped<GetGreetingQueryHandler>();
        services.AddScoped<ListGreetingsQueryHandler>();

        return services;
    }
}
