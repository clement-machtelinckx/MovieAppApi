using MovieAppApi.Src.Domain.Entities;

namespace MovieAppApi.Src.Domain.Interfaces;

public interface IWatchlistRepository
{
    Task<IReadOnlyCollection<WatchlistItem>> GetAllAsync();
    Task<WatchlistItem?> GetByMovieIdAsync(int movieId);
    Task<WatchlistItem> AddAsync(WatchlistItem item);
    Task<bool> RemoveByMovieIdAsync(int movieId);
    Task<WatchlistItem?> SetWatchedAsync(int movieId, bool isWatched);
}
