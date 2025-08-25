using GameDataService.Models;
using GameDataService.Repositories;

namespace GameDataService.Repositories;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<Game?> GetGameWithDetailsAsync(int id);
    Task<IEnumerable<Game>> GetAllGamesWithDetailsAsync();
}
