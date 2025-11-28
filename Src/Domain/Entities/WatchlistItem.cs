namespace MovieAppApi.Src.Domain.Entities;

public class WatchlistItem
{
    public int Id { get; set; }                 // PK
    public int MovieId { get; set; }            // ID TMDB
    public string Title { get; set; } = "";     // Titre du film
    public string PosterUrl { get; set; } = ""; // URL complète de l'affiche
    public DateTime AddedAt { get; set; }       // Date d'ajout dans la watchlist
    public bool IsWatched { get; set; }         // Vu ou pas
}
