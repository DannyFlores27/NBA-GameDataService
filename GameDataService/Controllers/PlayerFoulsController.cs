using GameDataService.Models;
using GameDataService.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameDataService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerFoulsController : ControllerBase
{
    private readonly IPlayerFoulService _playerFoulService;

    public PlayerFoulsController(IPlayerFoulService playerFoulService)
    {
        _playerFoulService = playerFoulService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerFoul>>> GetAllPlayerFouls()
    {
        var playerFouls = await _playerFoulService.GetAllAsync();
        return Ok(playerFouls);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerFoul>> GetPlayerFoulById(int id)
    {
        var playerFoul = await _playerFoulService.GetByIdAsync(id);
        if (playerFoul == null)
        {
            return NotFound();
        }
        return Ok(playerFoul);
    }

    [HttpPost]
    public async Task<ActionResult<PlayerFoul>> CreatePlayerFoul(PlayerFoul playerFoul)
    {
        var createdPlayerFoul = await _playerFoulService.AddAsync(playerFoul);
        return CreatedAtAction(
            nameof(GetPlayerFoulById),
            new { id = createdPlayerFoul.PlayerFoulId },
            createdPlayerFoul
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PlayerFoul>> UpdatePlayerFoul(int id, PlayerFoul playerFoul)
    {
        if (id != playerFoul.PlayerFoulId)
        {
            return BadRequest();
        }

        var updatedPlayerFoul = await _playerFoulService.UpdateAsync(playerFoul);
        if (updatedPlayerFoul == null)
        {
            return NotFound();
        }

        return Ok(updatedPlayerFoul);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlayerFoul(int id)
    {
        var result = await _playerFoulService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
