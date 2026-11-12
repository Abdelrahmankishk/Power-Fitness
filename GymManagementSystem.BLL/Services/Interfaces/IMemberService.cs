using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IMemberService
{
    Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct = default);

    Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);

    Task<MemberViewModel> GetMemberDetailsById(int id, CancellationToken ct = default);

    Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int id, CancellationToken ct = default);  
}
