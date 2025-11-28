namespace MovieAppApi.Src.Application.Dto;

public class MovieSearchResultItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Overview { get; set; } = "";
    public string? PosterPath { get; set; }
}

public class MovieSearchResultDto
{
    public int Page { get; set; }
    public List<MovieSearchResultItemDto> Results { get; set; } = [];
}
