using GymManagementSystem.BLL.ViewModels.PlanViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IPlanService
{
    Task<IEnumerable<PLanViewModel>> GetAllPlansAsync(CancellationToken ct = default);

    Task<PLanViewModel> GetPlanByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ChangePlanStatus(int id , CancellationToken ct = default);

    Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default);

    Task<bool> UpdatePlanDetailsAsync(int id, UpdatePlanViewModel UpdatedPlan, CancellationToken ct = default);
}
