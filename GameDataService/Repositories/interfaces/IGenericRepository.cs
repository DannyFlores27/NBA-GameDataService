using System.Linq.Expressions;

namespace GameDataService.Repositories.interfaces;


public interface IGenericRepository<T> where T : class
{
    Task<T?> GetById(int id);
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? predicate = null);
}
