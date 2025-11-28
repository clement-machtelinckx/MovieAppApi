using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Domain.Entities;
using MovieAppApi.Src.Domain.Interfaces;
using MovieAppApi.Src.Infrastructure.Persistence;

namespace MovieAppApi.Src.Infrastructure.Repositories;

public class WatchlistRepository : IWatchlistRepository
{
    private readonly AppDbContext _context;

    public WatchlistRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<WatchlistItem>> GetAllAsync()
    {
        return await _context.Watchlist
            .AsNoTracking()
            .OrderByDescending(w => w.AddedAt)
            .ToListAsync();
    }

    public async Task<WatchlistItem?> GetByMovieIdAsync(int movieId)
    {
        return await _context.Watchlist
            .FirstOrDefaultAsync(w => w.MovieId == movieId);
    }

    public async Task<WatchlistItem> AddAsync(WatchlistItem item)
    {
        item.AddedAt = DateTime.UtcNow;
        _context.Watchlist.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> RemoveByMovieIdAsync(int movieId)
    {
        var existing = await _context.Watchlist
            .FirstOrDefaultAsync(w => w.MovieId == movieId);

        if (existing is null) return false;

        _context.Watchlist.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<WatchlistItem?> SetWatchedAsync(int movieId, bool isWatched)
    {
        var existing = await _context.Watchlist
            .FirstOrDefaultAsync(w => w.MovieId == movieId);

        if (existing is null) return null;

        existing.IsWatched = isWatched;
        await _context.SaveChangesAsync();

        return existing;
    }
}
