using GameDataService.Models;
using GameDataService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameDataService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController(IGameService svc) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Game>> Create([FromBody] CreateGameDto dto)
    {
        try
        {
            var game = await svc.CreateGameAsync(dto);
            return Ok(game);
        }
        catch (InvalidOperationException ex)
        {
            // Devuelve un 409 Conflict con el mensaje de error
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Game>> Get(int id)
        => (await svc.GetAsync(id)) is { } g ? Ok(g) : NotFound();

    // Puntuación
    [HttpPost("{id:int}/score/home")]
    public async Task<ActionResult<Game>> HomePoints(int id, [FromBody] PointsDto dto)
        => Ok(await svc.AddPointsAsync(id, home: true, dto.Points));

    [HttpPost("{id:int}/score/visitor")]
    public async Task<ActionResult<Game>> VisitorPoints(int id, [FromBody] PointsDto dto)
        => Ok(await svc.AddPointsAsync(id, home: false, dto.Points));

    [HttpPost("{id:int}/score/home/decrement")]
    public async Task<ActionResult<Game>> HomeMinus(int id) => Ok(await svc.SubtractPointAsync(id, home: true));

    [HttpPost("{id:int}/score/visitor/decrement")]
    public async Task<ActionResult<Game>> VisitorMinus(int id) => Ok(await svc.SubtractPointAsync(id, home: false));

    // Faltas de equipo
    [HttpPost("{id:int}/fouls/team/{teamId:int}/inc")]
    public async Task<ActionResult<Game>> TeamFoulInc(int id, int teamId, [FromQuery] int period)
        => Ok(await svc.TeamFoulAsync(id, teamId, period, +1));

    [HttpPost("{id:int}/fouls/team/{teamId:int}/dec")]
    public async Task<ActionResult<Game>> TeamFoulDec(int id, int teamId, [FromQuery] int period)
        => Ok(await svc.TeamFoulAsync(id, teamId, period, -1));

    // Faltas de jugador
    [HttpPost("{id:int}/fouls/player/{playerId:int}/inc")]
    public async Task<ActionResult<Game>> PlayerFoulInc(int id, int playerId, [FromQuery] int period)
        => Ok(await svc.PlayerFoulAsync(id, playerId, period, +1));

    [HttpPost("{id:int}/fouls/player/{playerId:int}/dec")]
    public async Task<ActionResult<Game>> PlayerFoulDec(int id, int playerId, [FromQuery] int period)
        => Ok(await svc.PlayerFoulAsync(id, playerId, period, -1));

    // Tiempo
    [HttpPost("{id:int}/start")]
    public async Task<ActionResult<Game>> Start(int id, [FromBody] TimeDto dto)
        => Ok(await svc.StartAsync(id, dto.PeriodSeconds));

    [HttpPost("{id:int}/pause")]
    public async Task<ActionResult<Game>> Pause(int id) => Ok(await svc.PauseAsync(id));

    [HttpPost("{id:int}/resume")]
    public async Task<ActionResult<Game>> Resume(int id) => Ok(await svc.ResumeAsync(id));

    [HttpPost("{id:int}/reset-period")]
    public async Task<ActionResult<Game>> ResetPeriod(int id, [FromBody] TimeDto dto)
        => Ok(await svc.ResetPeriodAsync(id, dto.PeriodSeconds));

    // Cuartos
    [HttpPost("{id:int}/next-period")]
    public async Task<ActionResult<Game>> NextPeriod(int id) => Ok(await svc.NextPeriodAsync(id));

    [HttpPost("{id:int}/previous-period")]
    public async Task<ActionResult<Game>> PreviousPeriod(int id) => Ok(await svc.PreviousPeriodAsync(id));

    // General
    [HttpPost("{id:int}/reset-game")]
    public async Task<ActionResult<Game>> ResetGame(int id) => Ok(await svc.ResetGameAsync(id));

    [HttpPost("{id:int}/suspend")]
    public async Task<ActionResult<Game>> Suspend(int id) => Ok(await svc.SuspendAsync(id));

    [HttpPost("{id:int}/finish")]
    public async Task<ActionResult<Game>> Finish(int id) => Ok(await svc.FinishGameAsync(id));

    // "Guardar": los cambios ya se guardan en cada acción; este endpoint es opcional/no-op.
    [HttpPost("{id:int}/save")]
    public async Task<ActionResult<Game>> Save(int id) => (await svc.GetAsync(id)) is { } g ? Ok(g) : NotFound();
}
