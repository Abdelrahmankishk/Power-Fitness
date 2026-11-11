using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IMemberService
{
    Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct);
}
