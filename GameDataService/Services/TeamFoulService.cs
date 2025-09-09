using GameDataService.Models;
using GameDataService.Repositories.interfaces;
using GameDataService.Services.interfaces;

namespace GameDataService.Services;

public class TeamFoulService(ITeamFoulRepository teamFoulRepo) : ITeamFoulService
{
    private readonly ITeamFoulRepository _teamFoulRepo = teamFoulRepo;

    public async Task<IEnumerable<TeamFoul>> GetAllAsync() => await _teamFoulRepo.GetAll();

    public async Task<TeamFoul?> GetByIdAsync(int id) => await _teamFoulRepo.GetById(id);

    public async Task<TeamFoul> AddAsync(TeamFoul teamFoul) => await _teamFoulRepo.Add(teamFoul);

    public async Task<TeamFoul?> UpdateAsync(TeamFoul teamFoul) =>
        await _teamFoulRepo.Update(teamFoul);

    public async Task<bool> DeleteAsync(int id) => await _teamFoulRepo.Delete(id);
}
