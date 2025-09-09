using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.Repositories;

public class TeamFoulRepository : ITeamFoulRepository
{
    private readonly ScoreboardDbContext _context;

    public TeamFoulRepository(ScoreboardDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TeamFoul>> GetAll()
    {
        return await _context.TeamFouls.Include(tf => tf.Game).Include(tf => tf.Team).ToListAsync();
    }

    public async Task<TeamFoul?> GetById(int id)
    {
        return await _context
            .TeamFouls.Include(tf => tf.Game)
            .Include(tf => tf.Team)
            .FirstOrDefaultAsync(tf => tf.TeamFoulId == id);
    }

    public async Task<TeamFoul> Add(TeamFoul teamFoul)
    {
        _context.TeamFouls.Add(teamFoul);
        await _context.SaveChangesAsync();
        return teamFoul;
    }

    public async Task<TeamFoul?> Update(TeamFoul teamFoul)
    {
        var existing = await _context.TeamFouls.FindAsync(teamFoul.TeamFoulId);
        if (existing == null)
            return null;
        _context.Entry(existing).CurrentValues.SetValues(teamFoul);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var teamFoul = await _context.TeamFouls.FindAsync(id);
        if (teamFoul == null)
            return false;
        _context.TeamFouls.Remove(teamFoul);
        await _context.SaveChangesAsync();
        return true;
    }
}
