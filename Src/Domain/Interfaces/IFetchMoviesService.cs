using MovieAppApi.Src.Application.Models;

namespace MovieAppApi.Src.Domain.Interfaces;

public interface IFetchMoviesService
{
    Task<SearchMoviesResultModel> SearchMoviesAsync(SearchMoviesRequestQueryModel request);
    Task<MovieModel> GetMovieAsync(int movieId, string language);
}
