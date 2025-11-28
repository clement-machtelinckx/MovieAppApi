using System.Net.Http.Headers;
using System.Text.Json;
using MovieAppApi.Src.Application.Models;
using MovieAppApi.Src.Domain.Exceptions;
using MovieAppApi.Src.Domain.Interfaces;

namespace MovieAppApi.Src.Infrastructure.External;

public class TmdbService : IFetchMoviesService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.themoviedb.org/3";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        var apiKey = configuration["TMDB_API_KEY"]
                     ?? throw new Exception("TMDB_API_KEY missing in configuration");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<SearchMoviesResultModel> SearchMoviesAsync(SearchMoviesRequestQueryModel request)
    {
        var url =
            $"{BaseUrl}/search/movie?query={Uri.EscapeDataString(request.SearchTerm)}&language={request.Language}";

        using var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"TMDB search failed with status code {(int)response.StatusCode} ({response.StatusCode})");
        }

        var json = await response.Content.ReadAsStringAsync();

        var tmdbResult = JsonSerializer.Deserialize<TmdbSearchResponse>(json, JsonOptions)
                         ?? new TmdbSearchResponse();

        return new SearchMoviesResultModel
        {
            Page = tmdbResult.Page,
            Results = tmdbResult.Results
                .Select(MapToMovieModel)
                .ToList()
        };
    }

    public async Task<MovieModel> GetMovieAsync(int movieId, string language)
    {
        var url = $"{BaseUrl}/movie/{movieId}?language={language}";

        using var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new MovieNotFoundException(movieId);
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"TMDB get movie failed with status code {(int)response.StatusCode} ({response.StatusCode})");
        }

        var json = await response.Content.ReadAsStringAsync();

        var tmdbMovie = JsonSerializer.Deserialize<TmdbMovie>(json, JsonOptions)
                        ?? throw new MovieNotFoundException(movieId);

        return MapToMovieModel(tmdbMovie);
    }

    private static MovieModel MapToMovieModel(TmdbMovie tmdbMovie)
    {
        return new MovieModel
        {
            Id = tmdbMovie.Id,
            Title = tmdbMovie.Title ?? string.Empty,
            Overview = tmdbMovie.Overview ?? string.Empty,
            PosterPath = tmdbMovie.PosterPath,
            OriginalLanguage = tmdbMovie.OriginalLanguage,
            ReleaseDate = tmdbMovie.ReleaseDate,
            VoteAverage = tmdbMovie.VoteAverage
        };
    }

    // DTO internes pour désérialiser la réponse TMDB
    private class TmdbSearchResponse
    {
        public int Page { get; set; }
        public List<TmdbMovie> Results { get; set; } = new();
    }

    private class TmdbMovie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? ReleaseDate { get; set; }
        public double VoteAverage { get; set; }
    }
}
