using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IPlanService
{
    Task<Result<IEnumerable<PLanViewModel>>> GetAllPlansAsync(CancellationToken ct = default);

    Task<Result<PLanViewModel>> GetPlanByIdAsync(int id, CancellationToken ct = default);
    Task<Result> ChangePlanStatus(int id , CancellationToken ct = default);

    Task<Result<UpdatePlanViewModel?>> GetPlanToUpdateAsync(int id, CancellationToken ct = default);

    Task<Result> UpdatePlanDetailsAsync(int id, UpdatePlanViewModel UpdatedPlan, CancellationToken ct = default);
}
