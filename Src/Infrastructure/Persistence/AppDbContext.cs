using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Domain.Entities;

namespace MovieAppApi.Src.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<WatchlistItem> Watchlist { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    }
}
