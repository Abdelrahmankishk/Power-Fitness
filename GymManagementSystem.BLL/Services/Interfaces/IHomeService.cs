using GymManagementSystem.BLL.ViewModels.HomeViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IHomeService
{
    Task<HomeViewModel> GetAllAnalytics(CancellationToken ct = default);
}
