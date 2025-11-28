using MovieAppApi.Src.Application.Dto;

namespace MovieAppApi.Src.Application.Interfaces;

public interface IWatchlistService
{
    Task<IReadOnlyCollection<WatchlistItemDto>> GetAllAsync();
    Task<WatchlistItemDto?> AddAsync(AddToWatchlistRequestDto input);
    Task<bool> RemoveAsync(int movieId);
    Task<WatchlistItemDto?> SetWatchedAsync(int movieId, bool isWatched);
}
