using GameDataService.Models;

namespace GameDataService.Services.interfaces;

public interface IPlayerService
{
    Task<IEnumerable<Player>> GetAllAsync();
    Task<Player?> GetByIdAsync(int id);
    Task<Player> AddAsync(Player player);
    Task<Player?> UpdateAsync(Player player);
    Task<bool> DeleteAsync(int id);
}
