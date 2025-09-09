using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataService.Models;

namespace GameDataService.Repositories.interfaces
{
    public interface IPlayerFoulRepository
    {
        Task<IEnumerable<PlayerFoul>> GetAll();
        Task<PlayerFoul?> GetById(int id);
        Task<PlayerFoul> Add(PlayerFoul playerFoul);
        Task<PlayerFoul?> Update(PlayerFoul playerFoul);
        Task<bool> Delete(int id);
    }
}
