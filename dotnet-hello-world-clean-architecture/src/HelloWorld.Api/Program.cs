using HelloWorld.Api.Observability;
using HelloWorld.Api.Composition;
using HelloWorld.Api.Results;
using HelloWorld.Api.Security;
using HelloWorld.Infrastructure;
using HelloWorld.Infrastructure.Resilience;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});

builder.AddHelloWorldOpenTelemetry();

builder.Services.Configure<RetryOptions>(
    builder.Configuration.GetSection("Retries:GreetingStorage"));

builder.Services.AddControllers();
builder.Services.AddSingleton<IApiResultMapper, ApiResultMapper>();
builder.Services.AddHelloWorldSecurity(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddHelloWorldApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
