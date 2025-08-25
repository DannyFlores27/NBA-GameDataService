using GameDataService.Data;
using GameDataService.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameDataService.Repositories;

public class GenericRepository<T>(ScoreboardDbContext ctx) : IGenericRepository<T> where T : class
{
    private readonly ScoreboardDbContext _ctx = ctx;
    private readonly DbSet<T> _set = ctx.Set<T>();

    public async Task<T?> GetByIdAsync(int id)
    {
        if (typeof(T) == typeof(GameDataService.Models.Team))
        {
            return await _set.Include("Players").FirstOrDefaultAsync(e => EF.Property<int>(e, "TeamId") == id) as T;
        }
        return await _set.FindAsync(id);
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (typeof(T) == typeof(GameDataService.Models.Team))
        {
            var query = _set.Include("Players");
            if (predicate is null)
                return await query.ToListAsync();
            return await query.Where(predicate).ToListAsync();
        }
        return predicate is null ? await _set.ToListAsync() : await _set.Where(predicate).ToListAsync();
    }
    public async Task AddAsync(T entity) => await _set.AddAsync(entity);
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);
    public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
}
