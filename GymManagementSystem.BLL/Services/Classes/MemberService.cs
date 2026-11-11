using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class MemberService(IGenericRepository<Member> _memberRepository) : IMemberService
{
    private readonly IGenericRepository<Member> memberRepository = _memberRepository;
    public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct)
    {
        var members = await memberRepository.GetAllAsync(ct: ct);
        if(!members.Any())
            return [];

            var memberViewModel = members.Select(m=> new MemberViewModel()
            {
                Name = m.Name,
                Email = m.Email,
                Gender = m.Gender.ToString(),
                Phone = m.Phone,
                Photo = m.Photo,
                Id = m.Id
            });

        return memberViewModel;
    }
}
