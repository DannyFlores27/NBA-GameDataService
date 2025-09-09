using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataService.Models;

namespace GameDataService.Repositories.interfaces
{
    public interface ITeamFoulRepository
    {
        Task<IEnumerable<TeamFoul>> GetAll();
        Task<TeamFoul?> GetById(int id);
        Task<TeamFoul> Add(TeamFoul teamFoul);
        Task<TeamFoul?> Update(TeamFoul teamFoul);
        Task<bool> Delete(int id);
    }
}
