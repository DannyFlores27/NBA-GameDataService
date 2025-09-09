using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ScoreboardDbContext _context;

    public TeamRepository(ScoreboardDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Team>> GetAll()
    {
        return await _context.Teams.Include(t => t.Players).ToListAsync();
    }

    public async Task<Team?> GetById(int id)
    {
        return await _context
            .Teams.Include(t => t.Players)
            .FirstOrDefaultAsync(t => t.TeamId == id);
    }

    public async Task<Team> Add(Team team)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        return team;
    }

    public async Task<Team?> Update(Team team)
    {
        var existing = await _context.Teams.FindAsync(team.TeamId);
        if (existing == null)
            return null;
        _context.Entry(existing).CurrentValues.SetValues(team);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team == null)
            return false;
        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();
        return true;
    }
}
