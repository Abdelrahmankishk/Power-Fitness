using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class SessionService(IUnitOfWork _unitOfWork, IMapper _mapper) : ISessionServices
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
    {
        var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct:ct);
        if (sessions == null || !sessions.Any())
            return null!;

        var ReturnedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
        foreach (var session in ReturnedSessions)
        {
            session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedPlacesAsync(session.Id, ct);
        }
        return ReturnedSessions;
    }
}
