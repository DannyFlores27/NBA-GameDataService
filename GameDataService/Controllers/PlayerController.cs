using GameDataService.Models;
using GameDataService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameDataService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController(IGenericRepository<Player> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> Get() => Ok(await repo.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Player>> Get(int id)
        => (await repo.GetByIdAsync(id)) is { } p ? Ok(p) : NotFound();

    [HttpPost]
    public async Task<ActionResult<Player>> Post([FromBody] Player player)
    {
        await repo.AddAsync(player);
        await repo.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = player.PlayerId }, player);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Player updated)
    {
        var existing = await repo.GetByIdAsync(id);
        if (existing is null) return NotFound();
        existing.TeamId = updated.TeamId;
        existing.JerseyNumber = updated.JerseyNumber;
        existing.FullName = updated.FullName;
        existing.Position = updated.Position;
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
