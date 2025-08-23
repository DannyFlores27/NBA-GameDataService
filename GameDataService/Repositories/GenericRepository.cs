using GameDataService.Data;
using GameDataService.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameDataService.Repositories;

public class GenericRepository<T>(ScoreboardDbContext ctx) : IGenericRepository<T> where T : class
{
    private readonly ScoreboardDbContext _ctx = ctx;
    private readonly DbSet<T> _set = ctx.Set<T>();

    public async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        => predicate is null ? await _set.ToListAsync() : await _set.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) => await _set.AddAsync(entity);
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);
    public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
}
