using GameDataService.Data;
using GameDataService.Models;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.Repositories;

public class GameRepository(ScoreboardDbContext ctx) : GenericRepository<Game>(ctx), IGameRepository
{
    private readonly ScoreboardDbContext _ctx = ctx;

    public async Task<Game?> GetGameWithDetailsAsync(int id) =>
        await _ctx.Games
            .Include(g => g.HomeTeam)
            .Include(g => g.AwayTeam)
            .Include(g => g.TeamFouls)
            .Include(g => g.PlayerFouls)
            .FirstOrDefaultAsync(g => g.GameId == id);

    public async Task<bool> IsTeamInActiveGameAsync(int teamId)
    {
        return await _ctx.Games.AnyAsync(g =>
            (g.HomeTeamId == teamId || g.AwayTeamId == teamId) &&
            (g.GameStatus == GameStatus.RUNNING || g.GameStatus == GameStatus.PAUSED)
        );
    }

}
