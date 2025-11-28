using MovieAppApi.Src.Application.Models;

namespace MovieAppApi.Src.Application.Interfaces;

public interface IPlaylistService
{
    Task<PlaylistModel> CreatePlaylistAsync(CreatePlaylistRequestBodyModel model);
    Task<IReadOnlyCollection<PlaylistModel>> GetPlaylistsAsync();
    Task<PlaylistModel> GetPlaylistAsync(int id);
    Task<PlaylistModel> UpdatePlaylistAsync(int id, PlaylistModel model);
    Task<bool> DeletePlaylistAsync(int id);
}
