using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IMemberService
{
    Task<Result<IEnumerable<MemberViewModel>>> GetAllMemberAsync(CancellationToken ct = default);

    Task<Result> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);

    Task<Result<MemberViewModel>> GetMemberDetailsById(int id, CancellationToken ct = default);

    Task<Result<HealthRecordViewModel?>> GetMemberHealthRecordAsync(int id, CancellationToken ct = default); 
    Task<Result<UpdateMemberViewModel?>> GetMemberToUpdateAsync(int id, CancellationToken ct = default);
    Task<Result> UpdateMemberDetailsAsync(int id, UpdateMemberViewModel member, CancellationToken ct = default);

    Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default);
}
