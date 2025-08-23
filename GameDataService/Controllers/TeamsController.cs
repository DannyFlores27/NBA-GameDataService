using GameDataService.Models;
using GameDataService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameDataService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController(IGenericRepository<Team> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Team>>> Get() => Ok(await repo.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Team>> Get(int id)
        => (await repo.GetByIdAsync(id)) is { } team ? Ok(team) : NotFound();

    [HttpPost]
    public async Task<ActionResult<Team>> Post([FromBody] Team team)
    {
        await repo.AddAsync(team);
        await repo.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = team.TeamId }, team);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Team updated)
    {
        var existing = await repo.GetByIdAsync(id);
        if (existing is null) return NotFound();
        existing.Name = updated.Name;
        existing.City = updated.City;
        existing.LogoUrl = updated.LogoUrl;
        repo.Update(existing);
        await repo.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await repo.GetByIdAsync(id);
        if (existing is null) return NotFound();
        repo.Remove(existing);
        await repo.SaveChangesAsync();
        return NoContent();
    }
}
