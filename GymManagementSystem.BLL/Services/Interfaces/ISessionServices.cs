using GymManagementSystem.BLL.ViewModels.SessionViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ISessionServices
{
    Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
}
