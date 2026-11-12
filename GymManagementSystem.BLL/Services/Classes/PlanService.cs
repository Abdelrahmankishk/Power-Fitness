using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes;

public class PlanService(IGenericRepository<Plan> _PLanRepository) : IPlanService
{
    private IGenericRepository<Plan> PlanRepository = _PLanRepository;
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
}
