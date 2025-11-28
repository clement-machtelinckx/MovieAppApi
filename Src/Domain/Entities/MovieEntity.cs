namespace MovieAppApi.Src.Domain.Entities;

public class MovieEntity
{
    // Id TMDB, utilisé comme PK
    public int Id { get; set; }

    public List<PlaylistJoinMovieEntity> PlaylistJoinMovies { get; set; } = new();
}
