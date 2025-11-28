using Microsoft.AspNetCore.Mvc;

namespace MovieAppApi.Src.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(new
        {
            status = "OK",
            message = "MovieApp API is running!",
            timestamp = DateTime.UtcNow
        });
    }
}
