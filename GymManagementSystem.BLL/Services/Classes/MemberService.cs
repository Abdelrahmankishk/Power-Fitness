using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class MemberService(IGenericRepository<Member> _memberRepository) : IMemberService
{
    private readonly IGenericRepository<Member> memberRepository = _memberRepository;

    public async Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
    {
        var emailExists = await memberRepository.AnyAsync(x=>x.Email == member.Email,ct);
        var PhoneExists = await memberRepository.AnyAsync(x=>x.Phone == member.Phone,ct);

        if (emailExists || PhoneExists)
            return false;

        var NewMember = new Member()
        {
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            Gender = member.Gender,
            DateOfBirth = member.DateOfBirth,
            Address = new Address()
            {
                BuildingNumber = member.BuildingNumber,
                City = member.City,
                Street = member.Street
            },
            HealthRecord = new HealthRecord()
            {
                Weight = member.HealthRecordViewModel.Weight,
                Height = member.HealthRecordViewModel.Height,
                BloodType = member.HealthRecordViewModel.BloodType,
                note = member.HealthRecordViewModel.Note
            }
        };

        var result = await memberRepository.AddAsync(NewMember);

        return result > 0;
    }

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
