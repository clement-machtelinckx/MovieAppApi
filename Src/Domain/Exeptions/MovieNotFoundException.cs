namespace MovieAppApi.Src.Domain.Exceptions;

public class MovieNotFoundException : Exception
{
    public int MovieId { get; }

    public MovieNotFoundException(int movieId)
        : base($"Movie id {movieId} not found")
    {
        MovieId = movieId;
    }
}
