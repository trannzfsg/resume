using HelloWorld.Application.Abstractions;
using HelloWorld.Application.Greetings;
using HelloWorld.Infrastructure.GreetingHistory;
using HelloWorld.Infrastructure.GreetingUseCases;
using HelloWorld.Infrastructure.Resilience;
using HelloWorld.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HelloWorld.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IIdGenerator, SystemIdGenerator>();
        services.AddSingleton<InMemoryGreetingStore>();
        services.AddSingleton<RetryExecutor>();
        services.AddSingleton<IGreetingReader>(serviceProvider =>
            new RetryingGreetingReader(
                serviceProvider.GetRequiredService<InMemoryGreetingStore>(),
                serviceProvider.GetRequiredService<RetryExecutor>()));
        services.AddSingleton<IGreetingWriter>(serviceProvider =>
            new RetryingGreetingWriter(
                serviceProvider.GetRequiredService<InMemoryGreetingStore>(),
                serviceProvider.GetRequiredService<RetryExecutor>()));

        services.AddScoped<IGetGreetingQueryHandler>(serviceProvider =>
            new LoggingGetGreetingQueryHandler(
                serviceProvider.GetRequiredService<GetGreetingQueryHandler>(),
                serviceProvider.GetRequiredService<ILogger<LoggingGetGreetingQueryHandler>>()));

        services.AddScoped<IListGreetingsQueryHandler>(serviceProvider =>
            new LoggingListGreetingsQueryHandler(
                serviceProvider.GetRequiredService<ListGreetingsQueryHandler>(),
                serviceProvider.GetRequiredService<ILogger<LoggingListGreetingsQueryHandler>>()));

        return services;
    }
}
