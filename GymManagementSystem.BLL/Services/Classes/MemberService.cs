using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Attachment;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes;

public class MemberService(IUnitOfWork _unitOfWork, IMapper _mapper, IAttachmentService _attachmentService) : IMemberService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;
    private readonly IAttachmentService attachmentService = _attachmentService;

    public async Task<Result> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
    {
        var emailExists = await unitOfWork.GetRepository<Member>().AnyAsync(x=>x.Email == member.Email,ct);
        var PhoneExists = await unitOfWork.GetRepository<Member>().AnyAsync(x=>x.Phone == member.Phone,ct);

        if (emailExists || PhoneExists)
            return Result.Validation("Email or Phone Already exits for other Member");

        var storedPhotoNameResult = await attachmentService.UploadAsync(member.PhotoFile.OpenReadStream(), member.PhotoFile.FileName, "MembersPhotos", ct);
        if (!storedPhotoNameResult.success)
            return Result.Fail(storedPhotoNameResult.message!);

        var NewMember = mapper.Map<Member>(member);
        NewMember.Photo = storedPhotoNameResult.Value;

        unitOfWork.GetRepository<Member>().Add(NewMember);
        var result=  await unitOfWork.SaveChangesAsync(ct);

        if (result > 0)
        {
            return Result.OK();
        }
        else
        {
            attachmentService.Delete(storedPhotoNameResult.Value, "MembersPhotos");
            return Result.Fail("Failed to Create Member");
        }
    }

    public async Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default)
    {
        var memeber = await unitOfWork.GetRepository<Member>().GetByIdAsync(id,ct);
        if(memeber == null) 
            return Result.NotFound("Member Not Found");

        var Memberships = await unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPlanAsync(m => m.MemberId == id && m.EndDate > DateTime.Now);
        if (Memberships.Count > 0)
            return Result.Fail("Can Not Delete Members with an Active Membership");
        var hasFuture = await unitOfWork.GetRepository<Booking>().AnyAsync(b=>b.MemberId == id && b.Session.StartDate > DateTime.Now);
        if(hasFuture) 
            return Result.Validation("Member has Upcoming booked Sessions");

        attachmentService.Delete(memeber.Photo!, "MembersPhotos");
        unitOfWork.GetRepository<Member>().Delete(memeber);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed to Delete Member");
    }

    public async Task<Result<IEnumerable<MemberViewModel>>> GetAllMemberAsync(CancellationToken ct)
    {
        var members = await unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);

        var memberViewModel = mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);

        return Result<IEnumerable<MemberViewModel>>.OK(memberViewModel);
    }

    public async Task<Result<MemberViewModel>> GetMemberDetailsById(int id, CancellationToken ct = default)
    {
        var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
        if (member == null)
            return Result<MemberViewModel>.NotFound("Member Not Found");

        var ReturnedMember = mapper.Map<Member,MemberViewModel>(member);
        var ActiveMembership = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(x=>x.MemberId == id && x.EndDate > DateTime.Now);
        if(ActiveMembership is not null)
        {
            var ActivePlan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(ActiveMembership.Id, ct);
            ReturnedMember.PlanName = ActivePlan?.Name;
            ReturnedMember.MembershipStartDate = ActiveMembership.CreatedAt.ToString();
            ReturnedMember.MembershipEndDate = ActiveMembership.EndDate.ToString();
        }

        return Result<MemberViewModel>.OK(ReturnedMember);
    }

    public async Task<Result<HealthRecordViewModel?>> GetMemberHealthRecordAsync(int id, CancellationToken ct = default)
    {
        var healthRecord = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(r => r.MemberId == id, ct: ct);
        if (healthRecord is null) return Result<HealthRecordViewModel?>.NotFound("Health Record Not Found");
        else
            return Result<HealthRecordViewModel?>.OK(mapper.Map<HealthRecordViewModel>(healthRecord));
    }

    public async Task<Result<UpdateMemberViewModel?>> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
    {
        var ReturnMember = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
        if (ReturnMember is null)
            return Result<UpdateMemberViewModel?>.NotFound("Member Not Found");
        else
            return Result<UpdateMemberViewModel?>.OK(mapper.Map<UpdateMemberViewModel>(ReturnMember));
    }

    public async Task<Result> UpdateMemberDetailsAsync(int id, UpdateMemberViewModel member, CancellationToken ct = default)
    {
        var GetMember = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
        if(GetMember is null) 
            return Result.NotFound("Member not Found");

        var emailExists = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == member.Email && x.Id != id, ct);
        var PhoneExists = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == member.Phone && x.Id != id, ct);

        if (emailExists || PhoneExists)
            return Result.Validation("Email or Phone Already exits for other Member");

        mapper.Map(member,GetMember);
        GetMember.UpdatedAt = DateTime.Now;

        unitOfWork.GetRepository<Member>().Update(GetMember);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.NotFound("Failed to Update Member");
    }
}
