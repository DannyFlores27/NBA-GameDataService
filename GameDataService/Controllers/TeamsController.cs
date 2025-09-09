using GameDataService.Models;
using GameDataService.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameDataService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Team>>> GetAllTeams()
    {
        var teams = await _teamService.GetAllAsync();
        return Ok(teams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Team>> GetTeamById(int id)
    {
        var team = await _teamService.GetByIdAsync(id);
        if (team == null)
        {
            return NotFound();
        }
        return Ok(team);
    }

    [HttpPost]
    public async Task<ActionResult<Team>> CreateTeam(Team team)
    {
        var createdTeam = await _teamService.AddAsync(team);
        return CreatedAtAction(nameof(GetTeamById), new { id = createdTeam.TeamId }, createdTeam);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Team>> UpdateTeam(int id, Team team)
    {
        if (id != team.TeamId)
        {
            return BadRequest();
        }

        var updatedTeam = await _teamService.UpdateAsync(team);
        if (updatedTeam == null)
        {
            return NotFound();
        }

        return Ok(updatedTeam);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeam(int id)
    {
        var result = await _teamService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
