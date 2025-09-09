using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataService.Models;

namespace GameDataService.Repositories.interfaces
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAll();
        Task<Player?> GetById(int id);
        Task<Player> Add(Player player);
        Task<Player?> Update(Player player);
        Task<bool> Delete(int id);
    }
}
