using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MovieAppApi.Src.Application.Dto;
using MovieAppApi.Src.Application.Interfaces;
using MovieAppApi.Src.Application.Models;
using MovieAppApi.Src.Domain.Exceptions;

namespace MovieAppApi.Src.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistsController(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    // POST /api/playlists
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlaylistRequestBodyDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var model = new CreatePlaylistRequestBodyModel
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            MovieIds = dto.MovieIds.Distinct().ToList()
        };

        var created = await _playlistService.CreatePlaylistAsync(model);

        var response = MapToDto(created);

        return CreatedAtAction(
            nameof(GetById),
            new { playlistId = response.Id },
            response);
    }

    // GET /api/playlists
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var playlists = await _playlistService.GetPlaylistsAsync();
        var dtos = playlists.Select(MapToDto).ToList();
        return Ok(dtos);
    }

    // GET /api/playlists/{playlistId}
    [HttpGet("{playlistId:int}")]
    public async Task<IActionResult> GetById([Range(1, int.MaxValue)] int playlistId)
    {
        try
        {
            var playlist = await _playlistService.GetPlaylistAsync(playlistId);
            return Ok(MapToDto(playlist));
        }
        catch (PlaylistNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT /api/playlists/{playlistId}
    [HttpPut("{playlistId:int}")]
    public async Task<IActionResult> Update(
        [Range(1, int.MaxValue)] int playlistId,
        [FromBody] PlaylistDto dto)
    {
        if (playlistId != dto.Id)
        {
            return BadRequest("Route id and body id must match.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var model = new PlaylistModel
        {
            Id = dto.Id,
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            MovieIds = dto.MovieIds.Distinct().ToList()
        };

        try
        {
            var updated = await _playlistService.UpdatePlaylistAsync(playlistId, model);
            return Ok(MapToDto(updated));
        }
        catch (PlaylistNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE /api/playlists/{playlistId}
    [HttpDelete("{playlistId:int}")]
    public async Task<IActionResult> Delete([Range(1, int.MaxValue)] int playlistId)
    {
        var deleted = await _playlistService.DeletePlaylistAsync(playlistId);
        return deleted ? NoContent() : NotFound();
    }

    private static PlaylistDto MapToDto(PlaylistModel model)
    {
        return new PlaylistDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            MovieIds = model.MovieIds.ToList()
        };
    }
}
