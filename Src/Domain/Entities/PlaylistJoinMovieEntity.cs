namespace MovieAppApi.Src.Domain.Entities;

public class PlaylistJoinMovieEntity : CommonEntity
{
    public int PlaylistId { get; set; }
    public PlaylistEntity Playlist { get; set; } = null!;

    public int MovieId { get; set; }
    public MovieEntity Movie { get; set; } = null!;
}
