using GameDataService.Models;

namespace GameDataService.Services.interfaces;

public interface ITeamService
{
    Task<IEnumerable<Team>> GetAllAsync();
    Task<Team?> GetByIdAsync(int id);
    Task<Team> AddAsync(Team team);
    Task<Team?> UpdateAsync(Team team);
    Task<bool> DeleteAsync(int id);
}
