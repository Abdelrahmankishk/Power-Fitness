using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes;

public class MemberService(IUnitOfWork _unitOfWork, IMapper _mapper) : IMemberService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
    {
        var emailExists = await unitOfWork.GetRepository<Member>().AnyAsync(x=>x.Email == member.Email,ct);
        var PhoneExists = await unitOfWork.GetRepository<Member>().AnyAsync(x=>x.Phone == member.Phone,ct);

        if (emailExists || PhoneExists)
            return false;

        var NewMember = mapper.Map<Member>(member);

        unitOfWork.GetRepository<Member>().Add(NewMember);
        var result=  await unitOfWork.SaveChangesAsync(ct);

        return result > 0;
    }

    public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default)
    {
        var memeber = await unitOfWork.GetRepository<Member>().GetByIdAsync(id,ct);
        if(memeber == null) 
            return false;

        var hasFuture = await unitOfWork.GetRepository<Booking>().AnyAsync(b=>b.MemberId == id && b.Session.StartDate > DateTime.Now);
        if(hasFuture) 
            return false;

 
        unitOfWork.GetRepository<Member>().Delete(memeber);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0;
    }

    public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct)
    {
        var members = await unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
        if(!members.Any())
            return [];

        var memberViewModel = mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);

        return memberViewModel;
    }

    public async Task<MemberViewModel> GetMemberDetailsById(int id, CancellationToken ct = default)
    {
        var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
        if (member == null)
            return null!;

        var ReturnedMember = mapper.Map<Member,MemberViewModel>(member);
        var ActiveMembership = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(x=>x.MemberId == id && x.EndDate > DateTime.Now);
        if(ActiveMembership is not null)
        {
            var ActivePlan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(ActiveMembership.Id, ct);
            ReturnedMember.PlanName = ActivePlan?.Name;
            ReturnedMember.MembershipStartDate = ActiveMembership.CreatedAt.ToString();
            ReturnedMember.MembershipEndDate = ActiveMembership.EndDate.ToString();
        }

        return ReturnedMember;
    }

    public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int id, CancellationToken ct = default)
    {
        var healthRecord = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(r => r.MemberId == id, ct: ct);
        if (healthRecord is null) return null;
        else
            return mapper.Map<HealthRecordViewModel>(healthRecord);
    }

    public async Task<UpdateMemberViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
    {
        var ReturnMember = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
        if (ReturnMember is null)
            return null;
        else
            return mapper.Map<UpdateMemberViewModel>(ReturnMember);
    }

    public async Task<bool> UpdateMemberDetailsAsync(int id, UpdateMemberViewModel member, CancellationToken ct = default)
    {
        var GetMember = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
        if(GetMember is null) 
            return false;

        var emailExists = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == member.Email && x.Id != id, ct);
        var PhoneExists = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == member.Phone && x.Id != id, ct);

        if (emailExists || PhoneExists)
            return false;

        mapper.Map(member,GetMember);
        GetMember.UpdatedAt = DateTime.Now;

        unitOfWork.GetRepository<Member>().Update(GetMember);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? true : false;
    }
}
