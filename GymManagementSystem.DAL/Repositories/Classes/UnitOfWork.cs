using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class UnitOfWork(GymDbContext _context) : IUnitOfWork
{
    private readonly GymDbContext context = _context;
    private readonly Dictionary<string, object> repositories = [];

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
    {
        var typeName = typeof(TEntity).Name;
        if (repositories.TryGetValue(typeName, out var repository)) { 
            return (IGenericRepository<TEntity>)repository;
        }
        else
        {
            var repo = new GenericRepository<TEntity>(context);
            repositories[typeName] = repo;
            return repo; 
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct);

}
