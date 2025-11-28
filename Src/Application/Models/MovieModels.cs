namespace MovieAppApi.Src.Application.Models;

public class SearchMoviesRequestQueryModel
{
    public string SearchTerm { get; }
    public string Language { get; }

    public SearchMoviesRequestQueryModel(string searchTerm, string language)
    {
        SearchTerm = searchTerm;
        Language = language;
    }
}

public class MovieModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Overview { get; set; } = "";
    public string? PosterPath { get; set; }
    public string? OriginalLanguage { get; set; }
    public string? ReleaseDate { get; set; }
    public double VoteAverage { get; set; }
}

public class SearchMoviesResultModel
{
    public int Page { get; set; }
    public IReadOnlyCollection<MovieModel> Results { get; set; } = Array.Empty<MovieModel>();
}
