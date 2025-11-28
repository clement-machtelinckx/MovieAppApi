using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MovieAppApi.Src.Application.Validation;


namespace MovieAppApi.Src.Application.Dto;

public class SearchMoviesRequestQueryDto
{
    [FromQuery(Name = "search_term")]
    [Required(AllowEmptyStrings = false)]
    public string SearchTerm { get; set; } = "";

    [FromQuery(Name = "language")]
    [Required]
    [AllowedStringValues("en", "fr")]
    public string Language { get; set; } = "en";
}

public class GetMovieRequestQueryDto
{
    [FromQuery(Name = "language")]
    [Required]
    [AllowedStringValues("en", "fr")]
    public string Language { get; set; } = "en";
}

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Overview { get; set; } = "";
    public string? PosterUrl { get; set; }
    public string? OriginalLanguage { get; set; }
    public string? ReleaseDate { get; set; }
    public double VoteAverage { get; set; }
}

public class SearchMoviesResponseDto
{
    public int Page { get; set; }
    public List<MovieDto> Results { get; set; } = [];
}
