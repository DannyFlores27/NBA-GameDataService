using GameDataService.Models;
using GameDataService.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameDataService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
    {
        var players = await _playerService.GetAllAsync();
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayerById(int id)
    {
        var player = await _playerService.GetByIdAsync(id);
        if (player == null)
        {
            return NotFound();
        }
        return Ok(player);
    }

    [HttpPost]
    public async Task<ActionResult<Player>> CreatePlayer(Player player)
    {
        var createdPlayer = await _playerService.AddAsync(player);
        return CreatedAtAction(
            nameof(GetPlayerById),
            new { id = createdPlayer.PlayerId },
            createdPlayer
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Player>> UpdatePlayer(int id, Player player)
    {
        if (id != player.PlayerId)
        {
            return BadRequest();
        }

        var updatedPlayer = await _playerService.UpdateAsync(player);
        if (updatedPlayer == null)
        {
            return NotFound();
        }

        return Ok(updatedPlayer);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlayer(int id)
    {
        var result = await _playerService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
