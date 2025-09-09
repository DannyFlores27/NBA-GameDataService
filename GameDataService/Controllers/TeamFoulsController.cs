using GameDataService.Models;
using GameDataService.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameDataService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamFoulsController : ControllerBase
{
    private readonly ITeamFoulService _teamFoulService;

    public TeamFoulsController(ITeamFoulService teamFoulService)
    {
        _teamFoulService = teamFoulService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamFoul>>> GetAllTeamFouls()
    {
        var teamFouls = await _teamFoulService.GetAllAsync();
        return Ok(teamFouls);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamFoul>> GetTeamFoulById(int id)
    {
        var teamFoul = await _teamFoulService.GetByIdAsync(id);
        if (teamFoul == null)
        {
            return NotFound();
        }
        return Ok(teamFoul);
    }

    [HttpPost]
    public async Task<ActionResult<TeamFoul>> CreateTeamFoul(TeamFoul teamFoul)
    {
        var createdTeamFoul = await _teamFoulService.AddAsync(teamFoul);
        return CreatedAtAction(
            nameof(GetTeamFoulById),
            new { id = createdTeamFoul.TeamFoulId },
            createdTeamFoul
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TeamFoul>> UpdateTeamFoul(int id, TeamFoul teamFoul)
    {
        if (id != teamFoul.TeamFoulId)
        {
            return BadRequest();
        }

        var updatedTeamFoul = await _teamFoulService.UpdateAsync(teamFoul);
        if (updatedTeamFoul == null)
        {
            return NotFound();
        }

        return Ok(updatedTeamFoul);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeamFoul(int id)
    {
        var result = await _teamFoulService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
