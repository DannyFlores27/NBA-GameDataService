using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ScoreboardDbContext _context;

    public PlayerRepository(ScoreboardDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Player>> GetAll()
    {
        return await _context.Players.Include(p => p.Team).ToListAsync();
    }

    public async Task<Player?> GetById(int id)
    {
        return await _context
            .Players.Include(p => p.Team)
            .FirstOrDefaultAsync(p => p.PlayerId == id);
    }

    public async Task<Player> Add(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<Player?> Update(Player player)
    {
        var existing = await _context.Players.FindAsync(player.PlayerId);
        if (existing == null)
            return null;
        _context.Entry(existing).CurrentValues.SetValues(player);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
            return false;
        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return true;
    }
}
