using GameDataService.Models;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.Data;

public class ScoreboardDbContext(DbContextOptions<ScoreboardDbContext> options) : DbContext(options)
{
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<TeamFoul> TeamFouls => Set<TeamFoul>();
    public DbSet<PlayerFoul> PlayerFouls => Set<PlayerFoul>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // GameStatus como string
        modelBuilder
            .Entity<Game>()
            .Property(g => g.GameStatus)
            .HasConversion<string>()
            .HasDefaultValue(GameStatus.NOT_STARTED);

        modelBuilder
            .Entity<Game>()
            .HasOne(g => g.HomeTeam)
            .WithMany()
            .HasForeignKey(g => g.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Game>()
            .HasOne(g => g.AwayTeam)
            .WithMany()
            .HasForeignKey(g => g.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unicidad por juego/equipo/período (TeamFoul)
        modelBuilder
            .Entity<TeamFoul>()
            .HasIndex(tf => new
            {
                tf.GameId,
                tf.TeamId,
                tf.Period,
            })
            .IsUnique();

        // Unicidad por juego/jugador/período (PlayerFoul)
        modelBuilder
            .Entity<PlayerFoul>()
            .HasIndex(pf => new
            {
                pf.GameId,
                pf.PlayerId,
                pf.Period,
            })
            .IsUnique();

        modelBuilder
            .Entity<Team>()
            .HasMany(t => t.Players)
            .WithOne(p => p.Team)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
