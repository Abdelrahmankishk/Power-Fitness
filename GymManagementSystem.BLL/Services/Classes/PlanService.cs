using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;
using System.Collections.Generic;

namespace GymManagementSystem.BLL.Services.Classes;

public class PlanService(IUnitOfWork _unitOfWork,IMapper _mapper) : IPlanService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<Result> ChangePlanStatus(int id, CancellationToken ct = default)
    {
        var Plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        if (Plan == null)
            return Result.NotFound("Plan Not Found");

        if (Plan.IsActive && await HasAnyMemeberships(id,ct))
            return Result.Validation("This Plan Has Active Memberships");

        Plan.IsActive = !Plan.IsActive;
        Plan.UpdatedAt = DateTime.UtcNow;
        unitOfWork.GetRepository<Plan>().Update(Plan);
        int result = await unitOfWork.SaveChangesAsync(ct);

        return result>0 ? Result.OK() : Result.Fail("Failed to Change Plan Status");
    }

    public async Task<Result<IEnumerable<PLanViewModel>>> GetAllPlansAsync(CancellationToken ct = default)
    {
        var plans = await unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
        if (!plans.Any())
            return Result<IEnumerable<PLanViewModel>>.NotFound("No Pans Found");

        var PlansViewModel = mapper.Map<IEnumerable<Plan>, IEnumerable<PLanViewModel>>(plans);
        return Result<IEnumerable<PLanViewModel>>.OK(PlansViewModel);
    }

    public async Task<Result<PLanViewModel>> GetPlanByIdAsync(int id, CancellationToken ct = default)
    {
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        if (plan == null)
            return Result<PLanViewModel>.NotFound("Plan Not Found");

        var PlanViewModel = mapper.Map<Plan, PLanViewModel>(plan);

        return Result<PLanViewModel>.OK(PlanViewModel);
    }

    public async Task<Result<UpdatePlanViewModel?>> GetPlanToUpdateAsync(int id, CancellationToken ct = default)
    {
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        if (plan == null || !plan.IsActive)
            return Result<UpdatePlanViewModel?>.NotFound("Plan Must be Active to Edit");
        if(await HasAnyMemeberships(id,ct))
            return Result<UpdatePlanViewModel?>.Fail("This Plan Has Active Membeships Can not Edit it");
        else
            return Result<UpdatePlanViewModel?>.OK( mapper.Map<Plan,UpdatePlanViewModel>(plan));

    }

    public async Task<Result> UpdatePlanDetailsAsync(int id, UpdatePlanViewModel UpdatedPlan, CancellationToken ct = default)
    {
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        var ActiveMembership = await HasAnyMemeberships(id,ct);
        if (ActiveMembership)
            return Result.Validation("This Plan Has Active Membeships Can not Edit it");

        if (plan == null)
            return Result.NotFound("Plan Not Found");

        mapper.Map(UpdatedPlan, plan);
        plan.UpdatedAt = DateTime.UtcNow;
        
        unitOfWork.GetRepository<Plan>().Update(plan);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed to Update Plan");
    }
    private async Task<bool> HasAnyMemeberships(int id, CancellationToken ct = default) 
        => await unitOfWork.GetRepository<MemberShip>().AnyAsync(x => x.PlanId == id && x.EndDate > DateTime.UtcNow, ct);
}
