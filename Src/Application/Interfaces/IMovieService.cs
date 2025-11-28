using MovieAppApi.Src.Application.Dto;

namespace MovieAppApi.Src.Application.Interfaces;

public interface IMovieService
{
    Task<MovieSearchResultDto?> SearchMoviesAsync(string query);
}
