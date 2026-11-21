using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.HomeViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class HomeService(IUnitOfWork _unitOfWork) : IHomeService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<HomeViewModel> GetAllAnalytics(CancellationToken ct = default)
    {
        var TimeNow = DateTime.UtcNow;
       var UpcomingSessions = await unitOfWork.GetRepository<Session>().CountAsync(s=>s.StartDate > TimeNow);
       var OngoingSessions = await unitOfWork.GetRepository<Session>().CountAsync(s=>s.StartDate <= TimeNow && s.EndDate >= TimeNow);
       var CompletedSessions = await unitOfWork.GetRepository<Session>().CountAsync(s => s.EndDate < TimeNow);
       var TotalMembers = await unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
       var TotalTrainers = await unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
       var ActiveMembers = await unitOfWork.GetRepository<MemberShip>().CountAsync(x => x.EndDate > TimeNow, ct);

        return new HomeViewModel()
        {
            ActiveMembers = ActiveMembers,
            CompletedSessions = CompletedSessions,
            OngoingSessions = OngoingSessions,
            TotalMembers = TotalMembers,
            TotalTrainers = TotalTrainers,
            UpcomingSessions = UpcomingSessions
        };
    }
}
