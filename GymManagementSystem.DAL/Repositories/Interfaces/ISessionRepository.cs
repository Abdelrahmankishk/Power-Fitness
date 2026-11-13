using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface ISessionRepository : IGenericRepository<Session>
{
    Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default);
    Task<int> GetCountOfBookedPlacesAsync(int sessionId, CancellationToken ct = default);
}
