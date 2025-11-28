namespace MovieAppApi.Src.Application.Dto;

public class WatchlistItemDto
{
    public int MovieId { get; set; }
    public string Title { get; set; } = "";
    public string PosterUrl { get; set; } = "";
    public DateTime AddedAt { get; set; }
    public bool IsWatched { get; set; }
}

public class AddToWatchlistRequestDto
{
    public int MovieId { get; set; }
    public string Title { get; set; } = "";
    public string? PosterPath { get; set; } // ex: "/aBc123.jpg" depuis TMDB
}
