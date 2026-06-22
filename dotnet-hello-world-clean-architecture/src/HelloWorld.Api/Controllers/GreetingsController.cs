using HelloWorld.Api.Models;
using HelloWorld.Api.Results;
using HelloWorld.Api.Security;
using HelloWorld.Application.Greetings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Api.Controllers;

[ApiController]
[Route("api/greetings")]
[Produces("application/json")]
public sealed class GreetingsController : ControllerBase
{
    private readonly IGetGreetingQueryHandler _getGreetingQueryHandler;
    private readonly IListGreetingsQueryHandler _listGreetingsQueryHandler;
    private readonly IApiResultMapper _apiResultMapper;

    public GreetingsController(
        IGetGreetingQueryHandler getGreetingQueryHandler,
        IListGreetingsQueryHandler listGreetingsQueryHandler,
        IApiResultMapper apiResultMapper)
    {
        _getGreetingQueryHandler = getGreetingQueryHandler;
        _listGreetingsQueryHandler = listGreetingsQueryHandler;
        _apiResultMapper = apiResultMapper;
    }

    [HttpGet(Name = "GetGreeting")]
    [Authorize(Policy = AuthorizationPolicies.GreetingRead)]
    [ProducesResponseType<GreetingResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GreetingResponse>> GetGreetingAsync(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await _getGreetingQueryHandler.HandleAsync(
            new GetGreetingQuery(name),
            cancellationToken);

        return _apiResultMapper.ToActionResult(
            this,
            result,
            GreetingResponse.FromDto,
            "name");
    }

    [HttpGet("history", Name = "ListGreetingHistory")]
    [Authorize(Policy = AuthorizationPolicies.GreetingHistoryRead)]
    [ProducesResponseType<GreetingHistoryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GreetingHistoryResponse>> ListGreetingHistoryAsync(
        [FromQuery] int? max,
        CancellationToken cancellationToken)
    {
        var requestedMaximum = max ?? ListGreetingsQuery.DefaultCount;
        var result = await _listGreetingsQueryHandler.HandleAsync(
            new ListGreetingsQuery(requestedMaximum),
            cancellationToken);

        return _apiResultMapper.ToActionResult(
            this,
            result,
            greetings => new GreetingHistoryResponse(
                greetings
                    .Select(GreetingResponse.FromDto)
                    .ToArray()),
            "max");
    }
}
