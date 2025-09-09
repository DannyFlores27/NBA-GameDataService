using System.Text.Json.Serialization;
using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Repositories;
using GameDataService.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

// using GameDataService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// entity framework
builder.Services.AddDbContext<ScoreboardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

// repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ITeamFoulRepository, TeamFoulRepository>();
builder.Services.AddScoped<IPlayerFoulRepository, PlayerFoulRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScoreboardDbContext>();
    db.Database.Migrate();

    if (!db.Teams.Any())
    {
        db.Teams.AddRange(
            new Team { Name = "Home", City = "Azul" },
            new Team { Name = "Visitor", City = "Rojo" }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.MapControllers();
app.Run();
