using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

public class GameRepository : IGameRepository
{
    private readonly ScoreboardDbContext _context;

    public GameRepository(ScoreboardDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Game>> GetAll()
    {
        return await _context.Games.ToListAsync();
    }

    public async Task<Game?> GetById(int id)
    {
        return await _context.Games.FindAsync(id);
    }

    public async Task<Game> Add(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }

    public async Task<Game?> Update(Game game)
    {
        var existingGame = await _context.Games.FindAsync(game.GameId);
        if (existingGame == null)
            return null;

        _context.Entry(existingGame).CurrentValues.SetValues(game);
        await _context.SaveChangesAsync();
        return existingGame;
    }

    public async Task<bool> Delete(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
            return false;

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
        return true;
    }
}
