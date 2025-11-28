using MovieAppApi.Src.Application.Interfaces;
using MovieAppApi.Src.Application.Models;
using MovieAppApi.Src.Domain.Interfaces;

namespace MovieAppApi.Src.Application.Services;

public class MovieService : IMovieService
{
    private readonly IFetchMoviesService _fetchMoviesService;

    public MovieService(IFetchMoviesService fetchMoviesService)
    {
        _fetchMoviesService = fetchMoviesService;
    }

    public Task<SearchMoviesResultModel> SearchMoviesAsync(SearchMoviesRequestQueryModel request)
    {
        return _fetchMoviesService.SearchMoviesAsync(request);
    }

    public Task<MovieModel> GetMovieAsync(int movieId, string language)
    {
        return _fetchMoviesService.GetMovieAsync(movieId, language);
    }
}
