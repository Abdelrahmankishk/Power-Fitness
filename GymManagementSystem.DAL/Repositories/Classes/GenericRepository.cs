using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class GenericRepository<TEntity>(GymDbContext _context) : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    private readonly GymDbContext context = _context;
    protected readonly DbSet<TEntity> dbSet = _context.Set<TEntity>();


    public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        dbSet.Add(entity);
        return await context.SaveChangesAsync(ct);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        return dbSet.AsNoTracking().AnyAsync(predicate, ct);
    }

    public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
    {
        dbSet.Remove(entity);
        return await context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
    {
        IQueryable<TEntity> query = tracking ? dbSet : dbSet.AsNoTracking();
        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)=> await dbSet.FindAsync(id, ct);
    

    public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        dbSet.Update(entity);
        return await context.SaveChangesAsync(ct);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default)
    {
        IQueryable<TEntity> query = tracking ? dbSet : dbSet.AsNoTracking();
        return await query.FirstOrDefaultAsync(predicate, ct);
    }
}
