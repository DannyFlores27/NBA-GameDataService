using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using GameDataService.Services.interfaces;

namespace GameDataService.Services;

public class PlayerService(IPlayerRepository playerRepo) : IPlayerService
{
    private readonly IPlayerRepository _playerRepo = playerRepo;

    public async Task<IEnumerable<Player>> GetAllAsync() => await _playerRepo.GetAll();

    public async Task<Player?> GetByIdAsync(int id) => await _playerRepo.GetById(id);

    public async Task<Player> AddAsync(Player player) => await _playerRepo.Add(player);

    public async Task<Player?> UpdateAsync(Player player) => await _playerRepo.Update(player);

    public async Task<bool> DeleteAsync(int id) => await _playerRepo.Delete(id);
}
