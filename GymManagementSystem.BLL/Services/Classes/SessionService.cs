using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class SessionService(IUnitOfWork _unitOfWork) : ISessionServices
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
    {
        var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct:ct);
        if (sessions == null || !sessions.Any())
            return null!;

        var ReturnedSessions = sessions.Select(s => new SessionViewModel()
        {
            Id = s.Id,
            Capacity = s.Capacity,
            Description = s.Description!,
            CategoryName = s.Category.CategoryName,
            TrainerName = s.Trainer.Name,
            EndDate = s.EndDate,
            StartDate = s.StartDate
        });
        foreach (var session in ReturnedSessions)
        {
            session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedPlacesAsync(session.Id, ct);
        }
        return ReturnedSessions;
    }
}
