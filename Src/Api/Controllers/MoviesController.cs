using Microsoft.AspNetCore.Mvc;
using MovieAppApi.Src.Application.Interfaces;

namespace MovieAppApi.Src.Api.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _service;

    public MoviesController(IMovieService service)
    {
        _service = service;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        var result = await _service.SearchMoviesAsync(query);

        if (result is null)
            return StatusCode(502, "TMDB API error");

        return Ok(result);
    }
}
