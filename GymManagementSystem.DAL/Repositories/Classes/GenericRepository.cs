using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class GenericRepository<TEntity>(GymDbContext _context) : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    private readonly GymDbContext context = _context;
    private readonly DbSet<TEntity> dbSet = _context.Set<TEntity>();


    public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        dbSet.Add(entity);
        return await context.SaveChangesAsync(ct);
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
}
