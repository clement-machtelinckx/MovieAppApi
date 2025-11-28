using DotNetEnv;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Application.Interfaces;
using MovieAppApi.Src.Application.Services;
using MovieAppApi.Src.Domain.Interfaces;
using MovieAppApi.Src.Infrastructure.External;
using MovieAppApi.Src.Infrastructure.Persistence;
using MovieAppApi.Src.Infrastructure.Repositories;




// Load .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Console.WriteLine("TMDB KEY = " + builder.Configuration["TMDB_API_KEY"]);


// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MovieApp API",
        Version = "v1"
    });
});

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// TMDB
builder.Services.AddHttpClient<IFetchMoviesService, TmdbService>();
builder.Services.AddScoped<IMovieService, MovieService>();

// Watchlist
builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();

// Playlists
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieApp API v1");
    });
}

app.MapControllers();
app.Run();
