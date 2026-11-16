using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IMembershipService
{
    Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);
    Task<IEnumerable<PlanSelectViewModel>> GetPlansForSelectAsync(CancellationToken ct = default);
    Task<IEnumerable<MemberSelectViewModel>> GetMembersForSelectAsync(CancellationToken ct = default);
    Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default);
    Task<Result> DeleteActiveMembershipAsync(int Id, CancellationToken ct = default);
}
