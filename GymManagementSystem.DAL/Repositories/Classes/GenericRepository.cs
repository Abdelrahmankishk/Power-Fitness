using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class GenericRepository<TEntity>(GymDbContext _context) : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    private readonly GymDbContext context = _context;
    protected readonly DbSet<TEntity> dbSet = _context.Set<TEntity>();


    public void Add(TEntity entity)
    {
        dbSet.Add(entity);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        return dbSet.AsNoTracking().AnyAsync(predicate, ct);
    }

    public void Delete(TEntity entity)
    {
        dbSet.Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? condition = null, bool tracking = false, CancellationToken ct = default)
    {
        IQueryable<TEntity> query = tracking ? dbSet : dbSet.AsNoTracking();
        if (condition is not null) query = query.Where(condition);
        return await query.ToListAsync(ct);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)=> await dbSet.FindAsync(id, ct);
    

    public void Update(TEntity entity)
    {
        dbSet.Update(entity);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default)
    {
        IQueryable<TEntity> query = tracking ? dbSet : dbSet.AsNoTracking();
        return await query.FirstOrDefaultAsync(predicate, ct);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? condition = null, CancellationToken ct = default)
    {
        if (condition == null)
            return await dbSet.AsNoTracking().CountAsync(ct);
        else
            return await dbSet.AsNoTracking().CountAsync(condition, ct);
    }
}
