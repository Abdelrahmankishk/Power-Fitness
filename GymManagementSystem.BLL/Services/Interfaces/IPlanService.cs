using GymManagementSystem.BLL.ViewModels.PlanViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IPlanService
{
    Task<IEnumerable<PLanViewModel>> GetAllPlansAsync(CancellationToken ct = default);

    Task<PLanViewModel> GetPlanByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ChangePlanStatus(int id , CancellationToken ct = default);
}
