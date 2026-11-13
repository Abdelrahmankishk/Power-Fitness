using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity: BaseEntity,new();
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
