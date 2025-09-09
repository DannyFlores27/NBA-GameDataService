using GameDataService.Models;

namespace GameDataService.Repositories.interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAll();
        Task<Game?> GetById(int id);
        Task<Game> Add(Game game);
        Task<Game?> Update(Game game);
        Task<bool> Delete(int id);
    }
}
