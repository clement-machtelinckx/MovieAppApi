using Microsoft.AspNetCore.Mvc;
using MovieAppApi.Src.Application.Dto;
using MovieAppApi.Src.Application.Interfaces;
using MovieAppApi.Src.Application.Models;
using MovieAppApi.Src.Domain.Exceptions;

namespace MovieAppApi.Src.Api.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    /// <summary>
    /// Search movies on TMDB.
    /// </summary>
    /// <remarks>
    /// GET /api/movies?search_term=inception&language=fr
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] SearchMoviesRequestQueryDto queryDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var model = new SearchMoviesRequestQueryModel(
            queryDto.SearchTerm.Trim(),
            queryDto.Language
        );

        var resultModel = await _movieService.SearchMoviesAsync(model);

        var responseDto = new SearchMoviesResponseDto
        {
            Page = resultModel.Page,
            Results = resultModel.Results
                .Select(MapToMovieDto)
                .ToList()
        };

        return Ok(responseDto);
    }

    /// <summary>
    /// Get movie details by TMDB id.
    /// </summary>
    /// <remarks>
    /// GET /api/movies/550?language=en
    /// </remarks>
    [HttpGet("{movieId:int}")]
    public async Task<IActionResult> GetById(int movieId, [FromQuery] GetMovieRequestQueryDto queryDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var model = await _movieService.GetMovieAsync(movieId, queryDto.Language);
            var dto = MapToMovieDto(model);
            return Ok(dto);
        }
        catch (MovieNotFoundException)
        {
            return NotFound($"Movie id {movieId} not found");
        }
    }

    private static MovieDto MapToMovieDto(MovieModel model)
    {
        string? posterUrl = null;

        if (!string.IsNullOrWhiteSpace(model.PosterPath))
        {
            posterUrl = $"https://image.tmdb.org/t/p/w500{model.PosterPath}";
        }

        return new MovieDto
        {
            Id = model.Id,
            Title = model.Title,
            Overview = model.Overview,
            PosterUrl = posterUrl,
            OriginalLanguage = model.OriginalLanguage,
            ReleaseDate = model.ReleaseDate,
            VoteAverage = model.VoteAverage
        };
    }
}
