using GameDataService.Models;

namespace GameDataService.Services.interfaces;

public interface IPlayerFoulService
{
    Task<IEnumerable<PlayerFoul>> GetAllAsync();
    Task<PlayerFoul?> GetByIdAsync(int id);
    Task<PlayerFoul> AddAsync(PlayerFoul playerFoul);
    Task<PlayerFoul?> UpdateAsync(PlayerFoul playerFoul);
    Task<bool> DeleteAsync(int id);
}
