using GameDataService.Models;
using GameDataService.Models.DTOs;

namespace GameDataService.Services.interfaces;

public interface IGameService
{
    Task<Game?> GetGame(int gameId);
    Task<IEnumerable<Game>> GetGames();
    Task<Game> CreateGame(GameWriteDto dto);

    Task<Game> AddPointsAsync(int gameId, bool home, int points);
    Task<Game> SubtractPointAsync(int gameId, bool home);

    // Task<Game> TeamFoulAsync(int gameId, int teamId, int period, int delta);
    // Task<Game> PlayerFoulAsync(int gameId, int playerId, int period, int delta);
    Task<Game> StartAsync(int gameId, int periodSeconds);
    Task<Game> PauseAsync(int gameId);
    Task<Game> ResumeAsync(int gameId);
    Task<Game> ResetPeriodAsync(int gameId, int periodSeconds);
    Task<Game> NextPeriodAsync(int gameId);
    Task<Game> PreviousPeriodAsync(int gameId);
    Task<Game> ResetGameAsync(int gameId);
    Task<Game> SuspendAsync(int gameId);
    Task<Game> FinishGameAsync(int gameId);
    // Task<bool> IsTeamInActiveGameAsync(int teamId);
}
