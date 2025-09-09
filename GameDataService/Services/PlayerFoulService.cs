using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using GameDataService.Services.interfaces;

namespace GameDataService.Services;

public class PlayerFoulService(IPlayerFoulRepository playerFoulRepo) : IPlayerFoulService
{
    private readonly IPlayerFoulRepository _playerFoulRepo = playerFoulRepo;

    public async Task<IEnumerable<PlayerFoul>> GetAllAsync() => await _playerFoulRepo.GetAll();

    public async Task<PlayerFoul?> GetByIdAsync(int id) => await _playerFoulRepo.GetById(id);

    public async Task<PlayerFoul> AddAsync(PlayerFoul playerFoul) =>
        await _playerFoulRepo.Add(playerFoul);

    public async Task<PlayerFoul?> UpdateAsync(PlayerFoul playerFoul) =>
        await _playerFoulRepo.Update(playerFoul);

    public async Task<bool> DeleteAsync(int id) => await _playerFoulRepo.Delete(id);
}
