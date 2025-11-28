using DotNetEnv;
using Microsoft.OpenApi.Models;
using MovieAppApi.Src.Application.Interfaces;
using MovieAppApi.Src.Application.Services;


// Load .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Console.WriteLine("TMDB KEY = " + builder.Configuration["TMDB_API_KEY"]);


// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// builder Services
builder.Services.AddHttpClient<IMovieService, MovieService>();

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
