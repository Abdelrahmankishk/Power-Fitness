using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ISessionServices
{
    Task<Result<SessionViewModel?>> GetSessionByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default);
    Task<Result> CreateSessionAsync(CreateSessionViewModel NewSession ,CancellationToken ct = default);
    Task<IEnumerable<TrainerSessionSelectViewModel>> GetTrainersFromSessionAsync(CancellationToken ct = default);
    Task<IEnumerable<CategorySessionSelectViewModel>> GetCategoriesFromSessionAsync(CancellationToken ct = default);
    Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int id , CancellationToken ct = default);
    Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel updatedSession  , CancellationToken ct = default);
}
