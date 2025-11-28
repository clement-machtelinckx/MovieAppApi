using Microsoft.AspNetCore.Mvc;
using MovieAppApi.Src.Application.Dto;
using MovieAppApi.Src.Application.Interfaces;

namespace MovieAppApi.Src.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WatchlistController : ControllerBase
{
    private readonly IWatchlistService _service;

    public WatchlistController(IWatchlistService service)
    {
        _service = service;
    }

    // GET /api/watchlist
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    // POST /api/watchlist
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddToWatchlistRequestDto input)
    {
        if (input.MovieId <= 0 || string.IsNullOrWhiteSpace(input.Title))
            return BadRequest("MovieId and Title are required.");

        var item = await _service.AddAsync(input);
        return Ok(item);
    }

    // DELETE /api/watchlist/{movieId}
    [HttpDelete("{movieId:int}")]
    public async Task<IActionResult> Remove(int movieId)
    {
        var ok = await _service.RemoveAsync(movieId);
        return ok ? NoContent() : NotFound();
    }

    // PATCH /api/watchlist/{movieId}/watched
    [HttpPatch("{movieId:int}/watched")]
    public async Task<IActionResult> SetWatched(int movieId, [FromQuery] bool isWatched = true)
    {
        var updated = await _service.SetWatchedAsync(movieId, isWatched);
        return updated is null ? NotFound() : Ok(updated);
    }
}
