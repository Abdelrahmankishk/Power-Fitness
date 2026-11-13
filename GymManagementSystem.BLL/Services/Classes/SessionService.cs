using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class SessionService(IUnitOfWork _unitOfWork, IMapper _mapper) : ISessionServices
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<Result> CreateSessionAsync(CreateSessionViewModel NewSession, CancellationToken ct = default)
    {
        if (NewSession.EndDate <= NewSession.StartDate)
            return Result.Validation("End Date Must be After Start Date");

        if (NewSession.StartDate <= DateTime.Now)
            return Result.Validation("Start Date must be in the Future");

        if (NewSession.Capacity <= 1 || NewSession.Capacity > 25)
            return Result.Validation("Capacity must be between 1 and 25");

        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(NewSession.TrainerId);
        if (trainer == null)
            return Result.NotFound("Trainer Not Found");

        var Category = await unitOfWork.GetRepository<Category>().GetByIdAsync(NewSession.CategoryId);
        if (Category == null) return Result.NotFound("Category Not Found");

        var isValid = Enum.TryParse<TrainerSpecialties>(Category.CategoryName, true,out var category);
        if(!isValid || trainer.Specialtie != category) 
            return Result.Validation("Can Not Assign this Session to the Trainer");

        var session = mapper.Map<CreateSessionViewModel, Session>(NewSession);

        unitOfWork.GetRepository<Session>().Add(session);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed to Create Session");
    }

    public async Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default)
    {
        var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct:ct);
        if (sessions == null || !sessions.Any())
            return Result<IEnumerable<SessionViewModel>>.NotFound("Seesion Not Found");

        var ReturnedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
        foreach (var session in ReturnedSessions)
        {
            session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedPlacesAsync(session.Id, ct);
        }
        return Result<IEnumerable<SessionViewModel>>.OK(ReturnedSessions);
    }

    public async Task<IEnumerable<CategorySessionSelectViewModel>> GetCategoriesFromSessionAsync(CancellationToken ct = default)
    {
        var result = await unitOfWork.GetRepository<Category>().GetAllAsync(ct:ct);
        return mapper.Map<IEnumerable<CategorySessionSelectViewModel>>(result);
    }

    public async Task<IEnumerable<TrainerSessionSelectViewModel>> GetTrainersFromSessionAsync(CancellationToken ct = default)
    {
        var result = await unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
        return mapper.Map<IEnumerable<TrainerSessionSelectViewModel>>(result);
    }
}
