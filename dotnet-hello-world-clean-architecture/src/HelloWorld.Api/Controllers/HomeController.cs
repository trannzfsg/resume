using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Api.Controllers;

[ApiController]
[Route("")]
[Produces("application/json")]
public sealed class HomeController : ControllerBase
{
    [HttpGet(Name = "GetHome")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() =>
        Ok(new
        {
            service = "HelloWorld.CleanArchitecture",
            purpose = "A deliberately well-structured .NET hello-world API.",
            tryThese = new[]
            {
                "GET /api/greetings",
                "GET /api/greetings?name=Tran with X-Api-Key",
                "GET /api/greetings/history?max=10 with X-Api-Key",
                "GET /health"
            }
        });
}
