using GameDataService.Models;
using GameDataService.Models.DTOs;
using GameDataService.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameDataService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController(IGameService gameService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<GameReadDto>> Create([FromBody] GameWriteDto dto)
    {
        try
        {
            var game = await gameService.CreateGame(dto);
            return Ok(MapToReadDto(game));
        }
        catch (InvalidOperationException ex)
        {
            // Devuelve un 409 Conflict con el mensaje de error
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameReadDto>>> GetAll()
    {
        var games = await gameService.GetGames();
        return Ok(games.Select(MapToReadDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameReadDto>> Get(int id) =>
        (await gameService.GetGame(id)) is { } g ? Ok(MapToReadDto(g)) : NotFound();

    // Puntuación
    [HttpPost("{id:int}/score/home")]
    public async Task<ActionResult<GameReadDto>> HomePoints(int id, [FromBody] PointsDto dto)
    {
        var game = await gameService.AddPointsAsync(id, home: true, dto.Points);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/score/visitor")]
    public async Task<ActionResult<GameReadDto>> VisitorPoints(int id, [FromBody] PointsDto dto)
    {
        var game = await gameService.AddPointsAsync(id, home: false, dto.Points);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/score/home/decrement")]
    public async Task<ActionResult<GameReadDto>> HomeMinus(int id)
    {
        var game = await gameService.SubtractPointAsync(id, home: true);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/score/visitor/decrement")]
    public async Task<ActionResult<GameReadDto>> VisitorMinus(int id)
    {
        var game = await gameService.SubtractPointAsync(id, home: false);
        return Ok(MapToReadDto(game));
    }

    // // Faltas de equipo
    // [HttpPost("{id:int}/fouls/team/{teamId:int}/inc")]
    // public async Task<ActionResult<GameReadDto>> TeamFoulInc(
    //     int id,
    //     int teamId,
    //     [FromQuery] int period
    // )
    // {
    //     var game = await gameService.TeamFoulAsync(id, teamId, period, +1);
    //     return Ok(MapToReadDto(game));
    // }

    // [HttpPost("{id:int}/fouls/team/{teamId:int}/dec")]
    // public async Task<ActionResult<GameReadDto>> TeamFoulDec(
    //     int id,
    //     int teamId,
    //     [FromQuery] int period
    // )
    // {
    //     var game = await gameService.TeamFoulAsync(id, teamId, period, -1);
    //     return Ok(MapToReadDto(game));
    // }

    // // Faltas de jugador
    // [HttpPost("{id:int}/fouls/player/{playerId:int}/inc")]
    // public async Task<ActionResult<GameReadDto>> PlayerFoulInc(
    //     int id,
    //     int playerId,
    //     [FromQuery] int period
    // )
    // {
    //     var game = await gameService.PlayerFoulAsync(id, playerId, period, +1);
    //     return Ok(MapToReadDto(game));
    // }

    // [HttpPost("{id:int}/fouls/player/{playerId:int}/dec")]
    // public async Task<ActionResult<GameReadDto>> PlayerFoulDec(
    //     int id,
    //     int playerId,
    //     [FromQuery] int period
    // )
    // {
    //     var game = await gameService.PlayerFoulAsync(id, playerId, period, -1);
    //     return Ok(MapToReadDto(game));
    // }

    // Tiempo
    [HttpPost("{id:int}/start")]
    public async Task<ActionResult<GameReadDto>> Start(int id, [FromBody] TimeDto dto)
    {
        var game = await gameService.StartAsync(id, dto.PeriodSeconds);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/pause")]
    public async Task<ActionResult<GameReadDto>> Pause(int id)
    {
        var game = await gameService.PauseAsync(id);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/resume")]
    public async Task<ActionResult<GameReadDto>> Resume(int id)
    {
        var game = await gameService.ResumeAsync(id);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/reset-period")]
    public async Task<ActionResult<GameReadDto>> ResetPeriod(int id, [FromBody] TimeDto dto)
    {
        var game = await gameService.ResetPeriodAsync(id, dto.PeriodSeconds);
        return Ok(MapToReadDto(game));
    }

    // Cuartos
    [HttpPost("{id:int}/next-period")]
    public async Task<ActionResult<GameReadDto>> NextPeriod(int id)
    {
        var game = await gameService.NextPeriodAsync(id);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/previous-period")]
    public async Task<ActionResult<GameReadDto>> PreviousPeriod(int id)
    {
        var game = await gameService.PreviousPeriodAsync(id);
        return Ok(MapToReadDto(game));
    }

    // General
    [HttpPost("{id:int}/reset-game")]
    public async Task<ActionResult<GameReadDto>> ResetGame(int id)
    {
        var game = await gameService.ResetGameAsync(id);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/suspend")]
    public async Task<ActionResult<GameReadDto>> Suspend(int id)
    {
        var game = await gameService.SuspendAsync(id);
        return Ok(MapToReadDto(game));
    }

    [HttpPost("{id:int}/finish")]
    public async Task<ActionResult<GameReadDto>> Finish(int id)
    {
        var game = await gameService.FinishGameAsync(id);
        return Ok(MapToReadDto(game));
    }

    // "Guardar": los cambios ya se guardan en cada acción; este endpoint es opcional/no-op.
    [HttpPost("{id:int}/save")]
    public async Task<ActionResult<GameReadDto>> Save(int id) =>
        (await gameService.GetGame(id)) is { } g ? Ok(MapToReadDto(g)) : NotFound();

    private static GameReadDto MapToReadDto(Game game)
    {
        return new GameReadDto
        {
            GameId = game.GameId,
            GameDate = game.GameDate,
            HomeTeamId = game.HomeTeamId,
            AwayTeamId = game.AwayTeamId,
            // HomeScore = game.HomeScore,
            // AwayScore = game.AwayScore,
            // GameStatus = game.GameStatus,
            // CurrentPeriod = game.CurrentPeriod,
            // RemainingTime = game.RemainingTime,
            // PeriodStartTime = game.PeriodStartTime,
            // HomeTeam =
            //     game.HomeTeam != null
            //         ? new TeamReadDto { TeamId = game.HomeTeam.TeamId, Name = game.HomeTeam.Name }
            //         : null,
            // AwayTeam =
            //     game.AwayTeam != null
            //         ? new TeamReadDto { TeamId = game.AwayTeam.TeamId, Name = game.AwayTeam.Name }
            //         : null,
            // TeamFouls =
            //     game.TeamFouls?.Select(tf => new TeamFoulReadDto
            //         {
            //             TeamId = tf.TeamId,
            //             Period = tf.Period,
            //             TotalFouls = tf.TotalFouls,
            //         })
            //         .ToList() ?? new List<TeamFoulReadDto>(),
            // PlayerFouls =
            //     game.PlayerFouls?.Select(pf => new PlayerFoulReadDto
            //         {
            //             PlayerId = pf.PlayerId,
            //             Period = pf.Period,
            //             FoulCount = pf.FoulCount,
            //         })
            //         .ToList() ?? new List<PlayerFoulReadDto>(),
        };
    }
}
