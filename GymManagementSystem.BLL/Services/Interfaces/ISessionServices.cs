using GymManagementSystem.BLL.ViewModels.SessionViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ISessionServices
{
    Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
    Task<bool> CreateSessionAsync(CreateSessionViewModel NewSession ,CancellationToken ct = default);
    Task<IEnumerable<TrainerSessionSelectViewModel>> GetTrainersFromSessionAsync(CancellationToken ct = default);
    Task<IEnumerable<CategorySessionSelectViewModel>> GetCategoriesFromSessionAsync(CancellationToken ct = default);
}
