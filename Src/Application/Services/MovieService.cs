using System.Net.Http.Headers;
using System.Text.Json;
using MovieAppApi.Src.Application.Dto;
using MovieAppApi.Src.Application.Interfaces;

namespace MovieAppApi.Src.Application.Services;

public class MovieService : IMovieService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public MovieService(HttpClient http, IConfiguration config)
    {
        _http = http;

        // récupère TMDB_API_KEY depuis .env
        _apiKey = config["TMDB_API_KEY"] 
                  ?? throw new Exception("TMDB_API_KEY missing in .env");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<MovieSearchResultDto?> SearchMoviesAsync(string query)
    {
        var url = $"https://api.themoviedb.org/3/search/movie?query={Uri.EscapeDataString(query)}";

        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<MovieSearchResultDto>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
