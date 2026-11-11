using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> AddAsync(TEntity entity, CancellationToken ct = default);
    Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default);
    Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default);
}
