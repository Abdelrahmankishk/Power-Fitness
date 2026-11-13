using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes;

public class PlanService(IPlanRepository _PLanRepository,IGenericRepository<MemberShip> _MembershipRepository) : IPlanService
{
    private readonly IPlanRepository PlanRepository = _PLanRepository;
    private readonly IGenericRepository<MemberShip> membershipRepository = _MembershipRepository;

    public async Task<bool> ChangePlanStatus(int id, CancellationToken ct = default)
    {
        var Plan = await PlanRepository.GetPlanWithMembers(id, ct);
        if (Plan == null)
            return false;

        if (Plan.IsActive && await HasAnyMemeberships(id,ct))
            return false;

        Plan.IsActive = !Plan.IsActive;
        Plan.UpdatedAt = DateTime.Now;
        int result = await  PlanRepository.UpdateAsync(Plan);

        return result>0 ? true : false;
    }

    public async Task<IEnumerable<PLanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
    {
        var plans = await PlanRepository.GetAllAsync(ct: ct);
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
        var plan = await PlanRepository.GetByIdAsync(id, ct);
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
        var plan = await PlanRepository.GetByIdAsync(id, ct);
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
        var plan = await PlanRepository.GetPlanWithMembers(id, ct);
        var ActiveMembership = await HasAnyMemeberships(id,ct);
        if (ActiveMembership)
            return false;

        if (plan == null)
            return false;

        plan.DurationDays = UpdatedPlan.DurationDays;
        plan.Price = UpdatedPlan.Price;
        plan.Description = UpdatedPlan.Description;
        plan.UpdatedAt = DateTime.Now;
        
        var result =  await PlanRepository.UpdateAsync(plan);
        return result > 0 ? true : false;
    }
    private async Task<bool> HasAnyMemeberships(int id, CancellationToken ct = default) 
        => await membershipRepository.AnyAsync(x => x.PlanId == id && x.EndDate > DateTime.UtcNow, ct);
}
