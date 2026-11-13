using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes;

public class PlanService(IUnitOfWork _unitOfWork,IMapper _mapper) : IPlanService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<bool> ChangePlanStatus(int id, CancellationToken ct = default)
    {
        var Plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        if (Plan == null)
            return false;

        if (Plan.IsActive && await HasAnyMemeberships(id,ct))
            return false;

        Plan.IsActive = !Plan.IsActive;
        Plan.UpdatedAt = DateTime.Now;
        unitOfWork.GetRepository<Plan>().Update(Plan);
        int result = await unitOfWork.SaveChangesAsync(ct);

        return result>0 ? true : false;
    }

    public async Task<IEnumerable<PLanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
    {
        var plans = await unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
        if (!plans.Any())
            return [];

        var PlansViewModel = mapper.Map<IEnumerable<Plan>, IEnumerable<PLanViewModel>>(plans);
        return PlansViewModel;
    }

    public async Task<PLanViewModel> GetPlanByIdAsync(int id, CancellationToken ct = default)
    {
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        if (plan == null)
            return null!;

        var PlanViewModel = mapper.Map<Plan, PLanViewModel>(plan);

        return PlanViewModel;
    }

    public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default)
    {
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        if (plan == null || !plan.IsActive)
            return null;
        if(await HasAnyMemeberships(id,ct))
            return null!;
        else
            return mapper.Map<Plan,UpdatePlanViewModel>(plan);

    }

    public async Task<bool> UpdatePlanDetailsAsync(int id, UpdatePlanViewModel UpdatedPlan, CancellationToken ct = default)
    {
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        var ActiveMembership = await HasAnyMemeberships(id,ct);
        if (ActiveMembership)
            return false;

        if (plan == null)
            return false;

        mapper.Map(UpdatedPlan, plan);
        plan.UpdatedAt = DateTime.Now;
        
        unitOfWork.GetRepository<Plan>().Update(plan);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? true : false;
    }
    private async Task<bool> HasAnyMemeberships(int id, CancellationToken ct = default) 
        => await unitOfWork.GetRepository<MemberShip>().AnyAsync(x => x.PlanId == id && x.EndDate > DateTime.UtcNow, ct);
}
