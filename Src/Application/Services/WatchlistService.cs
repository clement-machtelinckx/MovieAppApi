using MovieAppApi.Src.Application.Dto;
using MovieAppApi.Src.Application.Interfaces;
using MovieAppApi.Src.Domain.Entities;
using MovieAppApi.Src.Domain.Interfaces;

namespace MovieAppApi.Src.Application.Services;

public class WatchlistService : IWatchlistService
{
    private readonly IWatchlistRepository _repo;

    public WatchlistService(IWatchlistRepository repo)
    {
        _repo = repo;
    }

    private static WatchlistItemDto ToDto(WatchlistItem item)
    {
        return new WatchlistItemDto
        {
            MovieId = item.MovieId,
            Title = item.Title,
            PosterUrl = item.PosterUrl,
            AddedAt = item.AddedAt,
            IsWatched = item.IsWatched
        };
    }

    public async Task<IReadOnlyCollection<WatchlistItemDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(ToDto).ToList();
    }

    public async Task<WatchlistItemDto?> AddAsync(AddToWatchlistRequestDto input)
    {
        // Empêcher les doublons
        var existing = await _repo.GetByMovieIdAsync(input.MovieId);
        if (existing is not null) return ToDto(existing);

        // Construire l'URL d'affiche depuis TMDB (optionnel)
        var posterUrl = input.PosterPath is not null
            ? $"https://image.tmdb.org/t/p/w500{input.PosterPath}"
            : "";

        var entity = new WatchlistItem
        {
            MovieId = input.MovieId,
            Title = input.Title,
            PosterUrl = posterUrl,
            IsWatched = false
        };

        var saved = await _repo.AddAsync(entity);
        return ToDto(saved);
    }

    public Task<bool> RemoveAsync(int movieId)
    {
        return _repo.RemoveByMovieIdAsync(movieId);
    }

    public async Task<WatchlistItemDto?> SetWatchedAsync(int movieId, bool isWatched)
    {
        var updated = await _repo.SetWatchedAsync(movieId, isWatched);
        return updated is null ? null : ToDto(updated);
    }
}
