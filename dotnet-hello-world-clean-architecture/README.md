# .NET 10 Clean Architecture Hello World

This is a deliberately over-engineered `Hello World` project for learning modern .NET 10 interview concepts in a small codebase.

It is not trying to be clever. It is trying to make the common enterprise patterns visible:

- Clean Architecture project boundaries
- dependency injection
- domain entities and value objects
- application use cases
- infrastructure adapters
- attribute-routed ASP.NET Core controllers
- API key authentication and policy-based authorization
- structured logging
- OpenTelemetry traces, metrics, and logs
- retry decorators around infrastructure operations
- validation
- unit tests
- SOLID principles

## Run It

This project is pinned to .NET SDK `10.0.301` through `global.json`.

This machine has a user-level .NET SDK at:

```powershell
C:\Users\USER\.dotnet\dotnet.exe
```

From this folder:

```powershell
C:\Users\USER\.dotnet\dotnet.exe test
C:\Users\USER\.dotnet\dotnet.exe run --project .\src\HelloWorld.Api\HelloWorld.Api.csproj
```

Then try:

```text
GET http://localhost:5000/
GET http://localhost:5000/api/greetings
GET http://localhost:5000/api/greetings?name=Tran
GET http://localhost:5000/api/greetings/history?max=10
GET http://localhost:5000/health
```

The actual port may differ. The console output will print the listening URL.

The greeting endpoints require an API key. Development uses `dev-api-key`:

```powershell
$headers = @{ "X-Api-Key" = "dev-api-key" }
Invoke-RestMethod "http://localhost:5000/api/greetings?name=Tran" -Headers $headers
Invoke-RestMethod "http://localhost:5000/api/greetings/history?max=10" -Headers $headers
```

For a non-development run, provide the key through configuration instead of committing it:

```powershell
$env:Authentication__ApiKey__ApiKey = "local-secret"
```

In Development, OpenTelemetry writes traces, metrics, and logs to the console so the signal is visible without extra tooling.
To send telemetry to a collector instead, set an OTLP endpoint before running:

```powershell
$env:OTEL_EXPORTER_OTLP_ENDPOINT = "http://localhost:4317"
C:\Users\USER\.dotnet\dotnet.exe run --project .\src\HelloWorld.Api\HelloWorld.Api.csproj
```

## Project Shape

```text
src/
  HelloWorld.Domain/
    The business rules. No dependency on ASP.NET, databases, files, or logging.

  HelloWorld.Application/
    The use cases. Coordinates domain objects through small interfaces.

  HelloWorld.Infrastructure/
    Framework and outside-world details. Clock, id generation, in-memory repository, logging and retry decorators.

  HelloWorld.Api/
    The composition root. HTTP endpoints, dependency injection setup, request/response models.

tests/
  HelloWorld.UnitTests/
    Tests for domain rules and application use cases.
```

Dependency direction:

```text
Api -> Application -> Domain
Api -> Infrastructure -> Application -> Domain
```

Domain does not know Application exists. Application does not know Infrastructure exists. Infrastructure implements Application interfaces. Api wires everything together.

## Step-By-Step Walkthrough

### 1. Start In The Domain

Open:

```text
src/HelloWorld.Domain/ValueObjects/RecipientName.cs
src/HelloWorld.Domain/ValueObjects/GreetingMessage.cs
src/HelloWorld.Domain/Entities/Greeting.cs
src/HelloWorld.Domain/Services/FriendlyGreetingComposer.cs
```

The important idea: invalid objects should be hard to create.

`RecipientName.Create(...)` trims input and rejects empty, too-long, or control-character values. The constructor is private, so callers must go through the factory method.

`Greeting.Create(...)` rejects an empty id and non-UTC timestamps. This is small, but it models a real rule: domain objects protect their own invariants.

### 2. Notice The Result Pattern

Open:

```text
src/HelloWorld.Domain/Common/Result.cs
src/HelloWorld.Domain/Common/Error.cs
src/HelloWorld.Domain/Errors/DomainErrors.cs
```

Instead of throwing exceptions for normal validation failures, the code returns `Result<T>`.

That gives interview-friendly behaviour:

- expected validation failures return controlled errors
- unexpected system failures can still throw exceptions
- handlers are explicit about success and failure paths

### 3. Move To The Application Layer

Open:

```text
src/HelloWorld.Application/Greetings/GetGreetingQueryHandler.cs
```

This is the main use case.

Read it top to bottom:

1. Validate the request.
2. Create a `RecipientName`.
3. Ask the domain composer for a message.
4. Create a `Greeting`.
5. Save it through `IGreetingWriter`.
6. Create a DTO through `IGreetingDtoFactory`.

The handler does not know whether greetings are stored in SQL Server, Postgres, a file, or memory. That is Dependency Inversion.

### 4. Study The Interfaces

Open:

```text
src/HelloWorld.Application/Abstractions/IClock.cs
src/HelloWorld.Application/Abstractions/IIdGenerator.cs
src/HelloWorld.Application/Abstractions/IGreetingReader.cs
src/HelloWorld.Application/Abstractions/IGreetingWriter.cs
src/HelloWorld.Application/Abstractions/IRequestValidator.cs
src/HelloWorld.Application/Greetings/IGreetingDtoFactory.cs
```

These interfaces are intentionally tiny. That is Interface Segregation.

The handler asks only for what it needs:

- current time
- new id
- save greetings
- list greetings
- validate a request
- create DTOs from domain entities

### 5. Look At Infrastructure

Open:

```text
src/HelloWorld.Infrastructure/Time/SystemClock.cs
src/HelloWorld.Infrastructure/Time/SystemIdGenerator.cs
src/HelloWorld.Infrastructure/GreetingHistory/InMemoryGreetingStore.cs
src/HelloWorld.Infrastructure/Resilience/RetryExecutor.cs
src/HelloWorld.Infrastructure/Resilience/RetryingGreetingReader.cs
src/HelloWorld.Infrastructure/Resilience/RetryingGreetingWriter.cs
src/HelloWorld.Infrastructure/GreetingUseCases/LoggingGetGreetingQueryHandler.cs
```

Infrastructure contains replaceable details.

`InMemoryGreetingStore` could later be replaced by a SQL-backed reader/writer without changing the application handlers.

`RetryingGreetingReader` and `RetryingGreetingWriter` are decorators. They retry transient infrastructure failures without forcing readers and writers into one broad interface.

`LoggingGetGreetingQueryHandler` is also a decorator. It wraps an `IGetGreetingQueryHandler`, not a concrete class, and adds logging without polluting the use case with framework logging.

### 6. See Dependency Injection

Open:

```text
src/HelloWorld.Api/Composition/ApplicationServiceCollectionExtensions.cs
src/HelloWorld.Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs
src/HelloWorld.Api/Program.cs
```

`Program.cs` is the composition root. This is where concrete things are wired together.

The key lines are:

```csharp
builder.Services.AddHelloWorldApplication();
builder.Services.AddInfrastructure();
```

That is where the app says:

- when something asks for `IGreetingReader`, use a retrying reader around in-memory storage
- when something asks for `IGreetingWriter`, use a retrying writer around in-memory storage
- when something asks for `IClock`, use `SystemClock`
- when something asks for `IGetGreetingQueryHandler`, use the logging decorator around the real handler

The Application project itself does not reference Microsoft dependency injection. The outer API composition root decides how application services are wired.

### 7. Read The API Last

Open:

```text
src/HelloWorld.Api/Controllers/GreetingsController.cs
src/HelloWorld.Api/Models/GreetingResponse.cs
src/HelloWorld.Api/Results/ApiResultMapper.cs
```

The controller layer converts HTTP into application requests:

```csharp
new GetGreetingQuery(name)
```

Then `IApiResultMapper` converts application results back to HTTP responses:

- success becomes `200 OK`
- validation failure becomes `400 Bad Request`

The API does not contain business rules. It is just transport.

The greeting actions use `[Authorize]` with named policies:

- `GreetingRead` requires the `greetings:read` scope
- `GreetingHistoryRead` requires the `greetings:history` scope

The root and health endpoints use `[AllowAnonymous]`.

## SOLID In This Project

Single Responsibility:

- `RecipientName` validates recipient names.
- `GetGreetingQueryHandler` coordinates the greeting use case.
- `GreetingDtoFactory` maps domain entities to application DTOs.
- `InMemoryGreetingStore` stores greetings.
- `RetryingGreetingReader` retries transient read failures.
- `RetryingGreetingWriter` retries transient write failures.
- `LoggingGetGreetingQueryHandler` logs the use case.
- `ApiKeyValidator` validates API keys.
- `ApiKeyClaimsFactory` creates authenticated principals.

Open/Closed:

- Add a new greeting composer or repository without rewriting the handler.
- Add another decorator for metrics or tracing without changing business logic.
- Add a different authentication scheme without changing controllers beyond policy attributes.
- Add new HTTP error mapping in `ApiResultMapper` without editing every controller.

Liskov Substitution:

- Anything implementing `IGreetingReader` should work wherever a reader is expected.
- Anything implementing `IGreetingWriter` should work wherever a writer is expected.
- SQL-backed storage should be substitutable for the in-memory storage as long as it honors those contracts.

Interface Segregation:

- `IClock`, `IIdGenerator`, `IGreetingReader`, `IGreetingWriter`, `IRequestValidator<T>`, and `IGreetingDtoFactory` are small focused interfaces.

Dependency Inversion:

- Application code depends on abstractions.
- Infrastructure code provides concrete implementations.

## Interview Practice

If someone asks you to review this design, say:

> This is intentionally more structured than a normal hello-world app. The point is to show separation of concerns. The Domain layer protects business rules, Application coordinates use cases, Infrastructure handles replaceable details, and Api wires the system together.

If someone asks what you would change for production:

- replace in-memory storage with a database adapter
- add integration tests around the API
- add authentication and authorization if the endpoint is not public
- replace the sample API-key scheme with OpenID Connect or JWT bearer tokens for a real user-facing API
- point OpenTelemetry at a real collector such as Azure Monitor, Grafana Tempo, Jaeger, or Honeycomb
- add API versioning if external clients depend on it
- add Docker and CI pipeline steps
- decide whether the logging decorators are enough or whether pipeline behaviours would be cleaner at larger scale
