using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes;

public class PlanService(IUnitOfWork _unitOfWork) : IPlanService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

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

        var PlansViewModel = plans.Select(p => new PLanViewModel()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            DurationDays = p.DurationDays,
            IsActive = p.IsActive,
            Price= p.Price
        });

        return PlansViewModel;
    }

    public async Task<PLanViewModel> GetPlanByIdAsync(int id, CancellationToken ct = default)
    {
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        if (plan == null)
            return null!;

        var PlanViewModel =  new PLanViewModel()
        {
            Name = plan.Name,
            Description = plan.Description,
            DurationDays = plan.DurationDays,
            IsActive = plan.IsActive,
            Price = plan.Price
        };

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
            return new UpdatePlanViewModel()
            {
                Name = plan.Name,
                Price = plan.Price,
                Description = plan.Description,
                DurationDays = plan.DurationDays
            };

    }

    public async Task<bool> UpdatePlanDetailsAsync(int id, UpdatePlanViewModel UpdatedPlan, CancellationToken ct = default)
    {
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
        var ActiveMembership = await HasAnyMemeberships(id,ct);
        if (ActiveMembership)
            return false;

        if (plan == null)
            return false;

        plan.DurationDays = UpdatedPlan.DurationDays;
        plan.Price = UpdatedPlan.Price;
        plan.Description = UpdatedPlan.Description;
        plan.UpdatedAt = DateTime.Now;
        
        unitOfWork.GetRepository<Plan>().Update(plan);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? true : false;
    }
    private async Task<bool> HasAnyMemeberships(int id, CancellationToken ct = default) 
        => await unitOfWork.GetRepository<MemberShip>().AnyAsync(x => x.PlanId == id && x.EndDate > DateTime.UtcNow, ct);
}
