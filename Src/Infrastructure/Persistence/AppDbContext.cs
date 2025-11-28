using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Domain.Entities;

namespace MovieAppApi.Src.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<WatchlistItem> Watchlist { get; set; } = null!;

    public DbSet<MovieEntity> Movies { get; set; } = null!;
    public DbSet<PlaylistEntity> Playlists { get; set; } = null!;
    public DbSet<PlaylistJoinMovieEntity> PlaylistJoinMovies { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Watchlist
        var watchlist = modelBuilder.Entity<WatchlistItem>();

        watchlist.ToTable("Watchlist");
        watchlist.HasKey(w => w.Id);

        watchlist.Property(w => w.MovieId)
            .IsRequired();

        watchlist.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(255);

        watchlist.Property(w => w.PosterUrl)
            .IsRequired(false);

        watchlist.Property(w => w.AddedAt)
            .IsRequired();

        watchlist.Property(w => w.IsWatched)
            .IsRequired();

        // MovieEntity
        var movie = modelBuilder.Entity<MovieEntity>();
        movie.ToTable("Movies");
        movie.HasKey(m => m.Id);

        // Id TMDB -> on ne génère pas en base
        movie.Property(m => m.Id)
            .ValueGeneratedNever();

        // PlaylistEntity
        var playlist = modelBuilder.Entity<PlaylistEntity>();
        playlist.ToTable("Playlists");
        playlist.HasKey(p => p.Id);

        playlist.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        playlist.Property(p => p.Description)
            .HasMaxLength(1000);

        playlist.Property(p => p.CreatedAt)
            .IsRequired();

        playlist.Property(p => p.UpdatedAt)
            .IsRequired();

        // PlaylistJoinMovieEntity
        var join = modelBuilder.Entity<PlaylistJoinMovieEntity>();
        join.ToTable("PlaylistJoinMovies");

        join.HasKey(j => new { j.PlaylistId, j.MovieId });

        join.Property(j => j.CreatedAt)
            .IsRequired();

        join.Property(j => j.UpdatedAt)
            .IsRequired();

        join.HasOne(j => j.Playlist)
            .WithMany(p => p.PlaylistJoinMovies)
            .HasForeignKey(j => j.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        // On ne veut pas supprimer le Movie quand on supprime la jointure
        join.HasOne(j => j.Movie)
            .WithMany(m => m.PlaylistJoinMovies)
            .HasForeignKey(j => j.MovieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
