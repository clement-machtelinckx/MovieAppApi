using MovieAppApi.Src.Application.Interfaces;
using MovieAppApi.Src.Application.Models;
using MovieAppApi.Src.Domain.Exceptions;
using MovieAppApi.Src.Domain.Interfaces;

namespace MovieAppApi.Src.Application.Services;

public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistRepository _repo;

    public PlaylistService(IPlaylistRepository repo)
    {
        _repo = repo;
    }

    public Task<PlaylistModel> CreatePlaylistAsync(CreatePlaylistRequestBodyModel model)
        => _repo.CreatePlaylistAsync(model);

    public Task<IReadOnlyCollection<PlaylistModel>> GetPlaylistsAsync()
        => _repo.GetPlaylistsAsync();

    public async Task<PlaylistModel> GetPlaylistAsync(int id)
    {
        var playlist = await _repo.GetPlaylistAsync(id);
        if (playlist is null)
        {
            throw new PlaylistNotFoundException(id);
        }

        return playlist;
    }

    public async Task<PlaylistModel> UpdatePlaylistAsync(int id, PlaylistModel model)
    {
        var updated = await _repo.UpdatePlaylistAsync(id, model);
        if (updated is null)
        {
            throw new PlaylistNotFoundException(id);
        }

        return updated;
    }

    public Task<bool> DeletePlaylistAsync(int id)
        => _repo.DeletePlaylistAsync(id);
}
