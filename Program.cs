using DotNetEnv;
using Microsoft.OpenApi.Models;
using MovieAppApi.Src.Application.Interfaces;
using MovieAppApi.Src.Application.Services;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Infrastructure.Persistence;
using MovieAppApi.Src.Domain.Interfaces;
using MovieAppApi.Src.Infrastructure.Repositories;




// Load .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Console.WriteLine("TMDB KEY = " + builder.Configuration["TMDB_API_KEY"]);


// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



// builder Services
builder.Services.AddHttpClient<IMovieService, MovieService>();

// DI Watchlist
builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MovieApp API",
        Version = "v1"
    });
});

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
