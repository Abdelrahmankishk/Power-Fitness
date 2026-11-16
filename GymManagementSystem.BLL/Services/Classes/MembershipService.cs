using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes;

public class MembershipService(IUnitOfWork _unitOfWork,IMapper _mapper) : IMembershipService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
    {
        var memberExists = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == model.MemberId, ct);
        if (!memberExists) 
            return Result.NotFound("Member Not Found");

        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId, ct);
        if (plan is null) 
            return Result.NotFound("Plan Not Found");
        if (!plan.IsActive) 
            return Result.Fail("Plan is Not Active");


        var hasActive = await unitOfWork.MembershipRepository
            .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
        if (hasActive) 
            return Result.Fail("Member  has an Active Membership");

        var entity = new MemberShip
        {
            MemberId = model.MemberId,
            PlanId = plan.Id,
            CreatedAt = DateTime.Now,
            EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.DurationDays),
        };

        unitOfWork.MembershipRepository.Add(entity);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed To Create New Membership");

    }

    public async Task<Result> DeleteActiveMembershipAsync(int Id, CancellationToken ct = default)
    {
        var active = await unitOfWork.MembershipRepository.FirstOrDefaultAsync( m => m.MemberId == Id && m.EndDate > DateTime.Now, tracking: true, ct: ct);

        if (active is null) 
            return Result.NotFound("No active membership for this member.");

        unitOfWork.MembershipRepository.Delete(active);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed To Delete Membership");

    }

    public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
    {
        var memberships = await unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPlanAsync(m => m.EndDate > DateTime.Now, ct);
        return mapper.Map<IEnumerable<MemberShip>,IEnumerable<MembershipViewModel>>(memberships);
    }

    public async Task<IEnumerable<MemberSelectViewModel>> GetMembersForSelectAsync(CancellationToken ct = default)
    {
        var members = await unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
        return mapper.Map<IEnumerable<Member>,IEnumerable<MemberSelectViewModel>>(members);
    }

    public async Task<IEnumerable<PlanSelectViewModel>> GetPlansForSelectAsync(CancellationToken ct = default)
    {
        var plans = await unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
        return mapper.Map<IEnumerable<Plan>,IEnumerable<PlanSelectViewModel>>(plans);
    }
}
