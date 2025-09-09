using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using GameDataService.Services.interfaces;

namespace GameDataService.Services;

public class TeamService(ITeamRepository teamRepo) : ITeamService
{
    private readonly ITeamRepository _teamRepo = teamRepo;

    public async Task<IEnumerable<Team>> GetAllAsync() => await _teamRepo.GetAll();

    public async Task<Team?> GetByIdAsync(int id) => await _teamRepo.GetById(id);

    public async Task<Team> AddAsync(Team team) => await _teamRepo.Add(team);

    public async Task<Team?> UpdateAsync(Team team) => await _teamRepo.Update(team);

    public async Task<bool> DeleteAsync(int id) => await _teamRepo.Delete(id);
}
