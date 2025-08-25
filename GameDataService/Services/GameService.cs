using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.Services;

public class GameService(IGameRepository gameRepo, ScoreboardDbContext ctx) : IGameService
{
    private readonly IGameRepository _gameRepo = gameRepo;
    private readonly ScoreboardDbContext _ctx = ctx;

    public async Task<bool> IsTeamInActiveGameAsync(int teamId)
    {
        return await _ctx.Games
            .AnyAsync(g =>
                (g.HomeTeamId == teamId || g.AwayTeamId == teamId) &&
                g.GameStatus != GameStatus.FINISHED &&
                g.GameStatus != GameStatus.SUSPENDED
            );
    }
    public async Task<Game> CreateGameAsync(CreateGameDto dto)
    {
        if (await IsTeamInActiveGameAsync(dto.HomeTeamId) || await IsTeamInActiveGameAsync(dto.AwayTeamId))
        {
            throw new InvalidOperationException("Uno de los equipos ya tiene un partido en curso.");
        }

        var game = new Game
        {
            GameDate = dto.GameDate,
            HomeTeamId = dto.HomeTeamId,
            AwayTeamId = dto.AwayTeamId,
            CurrentPeriod = 1,
            GameStatus = GameStatus.NOT_STARTED,
            RemainingTime = dto.PeriodSeconds ?? 600
        };

        await _gameRepo.AddAsync(game);
        await _gameRepo.SaveChangesAsync();

        return game;
    }


    public Task<Game?> GetAsync(int gameId) => _gameRepo.GetGameWithDetailsAsync(gameId);

    public async Task<Game> AddPointsAsync(int gameId, bool home, int points)
    {
        var game = await RequireGame(gameId);
        if (home) game.HomeScore += points; else game.AwayScore += points;
        _gameRepo.Update(game);
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> SubtractPointAsync(int gameId, bool home)
    {
        var game = await RequireGame(gameId);
        if (home) game.HomeScore = Math.Max(0, game.HomeScore - 1);
        else game.AwayScore = Math.Max(0, game.AwayScore - 1);
        _gameRepo.Update(game);
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> TeamFoulAsync(int gameId, int teamId, int period, int delta)
    {
        var game = await RequireGame(gameId);

        var tf = await _ctx.TeamFouls.FirstOrDefaultAsync(x =>
            x.GameId == gameId && x.TeamId == teamId && x.Period == period);

        if (tf is null)
        {
            tf = new TeamFoul { GameId = gameId, TeamId = teamId, Period = period, TotalFouls = 0 };
            await _ctx.TeamFouls.AddAsync(tf);
        }
        tf.TotalFouls = Math.Max(0, tf.TotalFouls + delta);

        await _ctx.SaveChangesAsync();
        return game;
    }

    public async Task<Game> PlayerFoulAsync(int gameId, int playerId, int period, int delta)
    {
        var game = await RequireGame(gameId);

        var pf = await _ctx.PlayerFouls.FirstOrDefaultAsync(x =>
            x.GameId == gameId && x.PlayerId == playerId && x.Period == period);

        if (pf is null)
        {
            pf = new PlayerFoul { GameId = gameId, PlayerId = playerId, Period = period, FoulCount = 0 };
            await _ctx.PlayerFouls.AddAsync(pf);
        }
        pf.FoulCount = Math.Max(0, pf.FoulCount + delta);

        await _ctx.SaveChangesAsync();
        return game;
    }

    public async Task<Game> StartAsync(int gameId, int periodSeconds)
    {
        var game = await RequireGame(gameId);
        game.GameStatus = GameStatus.RUNNING;
        game.CurrentPeriod = Math.Max(1, game.CurrentPeriod);
        game.RemainingTime = periodSeconds;
        game.PeriodStartTime = DateTime.UtcNow;
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> PauseAsync(int gameId)
    {
        var game = await RequireGame(gameId);
        if (game.GameStatus == GameStatus.RUNNING && game.PeriodStartTime.HasValue && game.RemainingTime.HasValue)
        {
            var elapsed = (int)Math.Max(0, (DateTime.UtcNow - game.PeriodStartTime.Value).TotalSeconds);
            game.RemainingTime = Math.Max(0, game.RemainingTime.Value - elapsed);
        }
        game.PeriodStartTime = null;
        game.GameStatus = GameStatus.PAUSED;
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> ResumeAsync(int gameId)
    {
        var game = await RequireGame(gameId);
        game.GameStatus = GameStatus.RUNNING;
        game.PeriodStartTime = DateTime.UtcNow;
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> ResetPeriodAsync(int gameId, int periodSeconds)
    {
        var game = await RequireGame(gameId);
        game.RemainingTime = periodSeconds;
        game.PeriodStartTime = null;
        game.GameStatus = GameStatus.PAUSED;
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> NextPeriodAsync(int gameId)
    {
        var game = await RequireGame(gameId);
        game.CurrentPeriod += 1;
        game.PeriodStartTime = null;
        game.GameStatus = GameStatus.PAUSED;
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> PreviousPeriodAsync(int gameId)
    {
        var game = await RequireGame(gameId);
        game.CurrentPeriod = Math.Max(1, game.CurrentPeriod - 1);
        game.PeriodStartTime = null;
        game.GameStatus = GameStatus.PAUSED;
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> ResetGameAsync(int gameId)
    {
        var game = await RequireGame(gameId);
        game.HomeScore = 0;
        game.AwayScore = 0;
        game.CurrentPeriod = 1;
        game.GameStatus = GameStatus.NOT_STARTED;
        game.RemainingTime = 600;
        game.PeriodStartTime = null;

        // Limpia faltas
        var tf = _ctx.TeamFouls.Where(x => x.GameId == gameId);
        var pf = _ctx.PlayerFouls.Where(x => x.GameId == gameId);
        _ctx.TeamFouls.RemoveRange(tf);
        _ctx.PlayerFouls.RemoveRange(pf);

        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> SuspendAsync(int gameId)
    {
        var game = await RequireGame(gameId);
        game.GameStatus = GameStatus.SUSPENDED;
        game.PeriodStartTime = null;
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    public async Task<Game> FinishGameAsync(int gameId)
    {
        var game = await RequireGame(gameId);
        game.GameStatus = GameStatus.FINISHED;
        game.RemainingTime = 0;
        game.PeriodStartTime = null;
        await _gameRepo.SaveChangesAsync();
        return game;
    }

    private async Task<Game> RequireGame(int id)
        => await _gameRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Game {id} not found");
}
