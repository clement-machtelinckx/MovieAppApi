using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Application.Models;
using MovieAppApi.Src.Domain.Entities;
using MovieAppApi.Src.Domain.Interfaces;
using MovieAppApi.Src.Infrastructure.Persistence;

namespace MovieAppApi.Src.Infrastructure.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly AppDbContext _context;

    public PlaylistRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PlaylistModel> CreatePlaylistAsync(CreatePlaylistRequestBodyModel model)
    {
        var now = DateTime.UtcNow;

        var movieIds = model.MovieIds
            .Distinct()
            .ToList();

        // S'assurer que les MovieEntity existent
        if (movieIds.Count > 0)
        {
            var existingMovies = await _context.Movies
                .Where(m => movieIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync();

            var missing = movieIds
                .Except(existingMovies)
                .ToList();

            foreach (var id in missing)
            {
                _context.Movies.Add(new MovieEntity { Id = id });
            }
        }

        var playlistEntity = new PlaylistEntity
        {
            Name = model.Name,
            Description = model.Description,
            CreatedAt = now,
            UpdatedAt = now
        };

        playlistEntity.PlaylistJoinMovies = movieIds
            .Select(id => new PlaylistJoinMovieEntity
            {
                Playlist = playlistEntity,
                MovieId = id,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        _context.Playlists.Add(playlistEntity);
        await _context.SaveChangesAsync();

        // Charger les jointures
        await _context.Entry(playlistEntity)
            .Collection(p => p.PlaylistJoinMovies)
            .LoadAsync();

        return ToModel(playlistEntity);
    }

    public async Task<IReadOnlyCollection<PlaylistModel>> GetPlaylistsAsync()
    {
        var entities = await _context.Playlists
            .Include(p => p.PlaylistJoinMovies)
            .AsNoTracking()
            .ToListAsync();

        return entities
            .Select(ToModel)
            .ToList();
    }

    public async Task<PlaylistModel?> GetPlaylistAsync(int id)
    {
        var entity = await _context.Playlists
            .Include(p => p.PlaylistJoinMovies)
            .FirstOrDefaultAsync(p => p.Id == id);

        return entity is null ? null : ToModel(entity);
    }

    public async Task<PlaylistModel?> UpdatePlaylistAsync(int id, PlaylistModel model)
    {
        var playlist = await _context.Playlists
            .Include(p => p.PlaylistJoinMovies)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (playlist is null)
        {
            return null;
        }

        playlist.Name = model.Name;
        playlist.Description = model.Description;
        playlist.UpdatedAt = DateTime.UtcNow;

        var newIds = model.MovieIds
            .Distinct()
            .ToList();

        var existingIds = playlist.PlaylistJoinMovies
            .Select(j => j.MovieId)
            .ToList();

        // Supprimer les jointures en trop
        var toRemove = playlist.PlaylistJoinMovies
            .Where(j => !newIds.Contains(j.MovieId))
            .ToList();

        foreach (var join in toRemove)
        {
            _context.PlaylistJoinMovies.Remove(join);
        }

        // Ajouter les nouvelles jointures
        var toAddIds = newIds
            .Except(existingIds)
            .ToList();

        if (toAddIds.Count > 0)
        {
            var existingMovies = await _context.Movies
                .Where(m => toAddIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync();

            var now = DateTime.UtcNow;

            var missing = toAddIds
                .Except(existingMovies)
                .ToList();

            foreach (var idMovie in missing)
            {
                _context.Movies.Add(new MovieEntity { Id = idMovie });
            }

            foreach (var idMovie in toAddIds)
            {
                playlist.PlaylistJoinMovies.Add(new PlaylistJoinMovieEntity
                {
                    PlaylistId = playlist.Id,
                    MovieId = idMovie,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }
        }

        await _context.SaveChangesAsync();

        await _context.Entry(playlist)
            .Collection(p => p.PlaylistJoinMovies)
            .LoadAsync();

        return ToModel(playlist);
    }

    public async Task<bool> DeletePlaylistAsync(int id)
    {
        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == id);

        if (playlist is null)
        {
            return false;
        }

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return true;
    }

    private static PlaylistModel ToModel(PlaylistEntity entity)
    {
        return new PlaylistModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            MovieIds = entity.PlaylistJoinMovies
                .Select(j => j.MovieId)
                .OrderBy(id => id)
                .ToList()
        };
    }
}
