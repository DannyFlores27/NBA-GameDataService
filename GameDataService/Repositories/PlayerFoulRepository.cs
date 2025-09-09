using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.Repositories;

public class PlayerFoulRepository : IPlayerFoulRepository
{
    private readonly ScoreboardDbContext _context;

    public PlayerFoulRepository(ScoreboardDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlayerFoul>> GetAll()
    {
        return await _context
            .PlayerFouls.Include(pf => pf.Game)
            .Include(pf => pf.Player)
            .ToListAsync();
    }

    public async Task<PlayerFoul?> GetById(int id)
    {
        return await _context
            .PlayerFouls.Include(pf => pf.Game)
            .Include(pf => pf.Player)
            .FirstOrDefaultAsync(pf => pf.PlayerFoulId == id);
    }

    public async Task<PlayerFoul> Add(PlayerFoul playerFoul)
    {
        _context.PlayerFouls.Add(playerFoul);
        await _context.SaveChangesAsync();
        return playerFoul;
    }

    public async Task<PlayerFoul?> Update(PlayerFoul playerFoul)
    {
        var existing = await _context.PlayerFouls.FindAsync(playerFoul.PlayerFoulId);
        if (existing == null)
            return null;
        _context.Entry(existing).CurrentValues.SetValues(playerFoul);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var playerFoul = await _context.PlayerFouls.FindAsync(id);
        if (playerFoul == null)
            return false;
        _context.PlayerFouls.Remove(playerFoul);
        await _context.SaveChangesAsync();
        return true;
    }
}
