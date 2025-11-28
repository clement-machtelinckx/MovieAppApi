namespace MovieAppApi.Src.Domain.Exceptions;

public class PlaylistNotFoundException : Exception
{
    public int PlaylistId { get; }

    public PlaylistNotFoundException(int playlistId)
        : base($"Playlist id {playlistId} not found")
    {
        PlaylistId = playlistId;
    }
}
