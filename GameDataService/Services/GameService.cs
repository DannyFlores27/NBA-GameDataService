using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Models.DTOs;
using GameDataService.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.Services.interfaces;

public class GameService(IGameRepository gameRepo, ScoreboardDbContext ctx) : IGameService
{
    private readonly IGameRepository _gameRepo = gameRepo;
    private readonly ScoreboardDbContext _ctx = ctx;

    // public async Task<bool> IsTeamInActiveGameAsync(int teamId)
    // {
    //     var activeStatuses = new[] { GameStatus.NOT_STARTED, GameStatus.RUNNING, GameStatus.PAUSED };

    // }

    public async Task<Game> CreateGame(GameWriteDto dto)
    {
        // if (await IsTeamInActiveGameAsync(dto.HomeTeamId) || await IsTeamInActiveGameAsync(dto.AwayTeamId))
        // {
        //     throw new InvalidOperationException("Uno de los equipos ya tiene un partido en curso.");
        // }

        var game = new Game
        {
            GameDate = dto.GameDate,
            HomeTeamId = dto.HomeTeamId,
            AwayTeamId = dto.AwayTeamId,
            CurrentPeriod = 1,
            GameStatus = GameStatus.NOT_STARTED,
            RemainingTime = dto.PeriodSeconds ?? 600,
        };

        await _gameRepo.Add(game);

        return game;
    }

    public Task<Game?> GetGame(int gameId) => _gameRepo.GetById(gameId);

    public Task<IEnumerable<Game>> GetGames() => _gameRepo.GetAll();

    public async Task<Game> AddPointsAsync(int gameId, bool home, int points)
    {
        var game = await getGameById(gameId);

        if (home)
            game.HomeScore += points;
        else
            game.AwayScore += points;
        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> SubtractPointAsync(int gameId, bool home)
    {
        var game = await getGameById(gameId);
        if (home)
            game.HomeScore = Math.Max(0, game.HomeScore - 1);
        else
            game.AwayScore = Math.Max(0, game.AwayScore - 1);
        await _gameRepo.Update(game);
        return game;
    }

    // public async Task<Game> TeamFoulAsync(int gameId, int teamId, int period, int delta)
    // {
    //     return await _gameRepo.AddTeamFoul(gameId, teamId, period, delta);
    // }

    // public async Task<Game> PlayerFoulAsync(int gameId, int playerId, int period, int delta)
    // {
    //     return await _gameRepo.AddPlayerFoul(gameId, playerId, period, delta);
    // }

    public async Task<Game> StartAsync(int gameId, int periodSeconds)
    {
        var game = await getGameById(gameId);
        game.GameStatus = GameStatus.RUNNING;
        game.CurrentPeriod = Math.Max(1, game.CurrentPeriod);
        game.RemainingTime = periodSeconds;
        game.PeriodStartTime = DateTime.UtcNow;
        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> PauseAsync(int gameId)
    {
        var game = await getGameById(gameId);
        if (
            game.GameStatus == GameStatus.RUNNING
            && game.PeriodStartTime.HasValue
            && game.RemainingTime.HasValue
        )
        {
            var elapsed = (int)
                Math.Max(0, (DateTime.UtcNow - game.PeriodStartTime.Value).TotalSeconds);
            game.RemainingTime = Math.Max(0, game.RemainingTime.Value - elapsed);
        }
        game.PeriodStartTime = null;
        game.GameStatus = GameStatus.PAUSED;
        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> ResumeAsync(int gameId)
    {
        var game = await getGameById(gameId);
        game.GameStatus = GameStatus.RUNNING;
        game.PeriodStartTime = DateTime.UtcNow;
        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> ResetPeriodAsync(int gameId, int periodSeconds)
    {
        var game = await getGameById(gameId);
        game.RemainingTime = periodSeconds;
        game.PeriodStartTime = null;
        game.GameStatus = GameStatus.PAUSED;
        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> NextPeriodAsync(int gameId)
    {
        var game = await getGameById(gameId);
        game.CurrentPeriod += 1;
        game.PeriodStartTime = null;
        game.GameStatus = GameStatus.PAUSED;
        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> PreviousPeriodAsync(int gameId)
    {
        var game = await getGameById(gameId);
        game.CurrentPeriod = Math.Max(1, game.CurrentPeriod - 1);
        game.PeriodStartTime = null;
        game.GameStatus = GameStatus.PAUSED;
        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> ResetGameAsync(int gameId)
    {
        var game = await getGameById(gameId);
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

        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> SuspendAsync(int gameId)
    {
        var game = await getGameById(gameId);
        game.GameStatus = GameStatus.SUSPENDED;
        game.PeriodStartTime = null;
        await _gameRepo.Update(game);
        return game;
    }

    public async Task<Game> FinishGameAsync(int gameId)
    {
        var game = await getGameById(gameId);
        game.GameStatus = GameStatus.FINISHED;
        game.RemainingTime = 0;
        game.PeriodStartTime = null;
        await _gameRepo.Update(game);
        return game;
    }

    private async Task<Game> getGameById(int id) =>
        await _gameRepo.GetById(id) ?? throw new KeyNotFoundException($"Game {id} not found");
}
