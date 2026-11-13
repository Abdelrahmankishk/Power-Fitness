using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class TrainerService(IUnitOfWork _unitOfWork, IMapper _mapper) : ITrainerService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default)
    {
        var emailExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == trainer.Email, ct);
        var PhoneExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == trainer.Phone, ct);

        if (emailExists || PhoneExists)
            return false;

        var newTrainer = mapper.Map<Trainer>(trainer);
        unitOfWork.GetRepository<Trainer>().Add(newTrainer);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? true : false;
    }

    public async Task<bool> DeleteTrainerAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id,ct);
        if (trainer == null) 
            return false;
        var HasSessions = await unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == id && s.StartDate > DateTime.Now);
        if(HasSessions)
            return false;

        unitOfWork.GetRepository<Trainer>().Delete(trainer);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0;
    }

    public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
    {
        var trainers = await unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
        if (!trainers.Any())
            return [];

        var TrainersViewModels = mapper.Map<IEnumerable<TrainerViewModel>>(trainers);

        return TrainersViewModels;
    }

    public async Task<TrainerViewModel> GetTrainerByIdAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
        if (trainer == null)
            return null!;

        var trainerViewModel = mapper.Map<TrainerViewModel>(trainer);
        return trainerViewModel;
    }

    public async Task<UpdateTrainerViewModel> GetTrainerToUpdateAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
        if (trainer == null)
            return null!;
        else
            return mapper.Map<UpdateTrainerViewModel>(trainer);
    }

    public async Task<bool> UpdateTrainerDetailsAsync(int id, UpdateTrainerViewModel UpdatedTrainer, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
        if (trainer == null)
            return false;
        var emailExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == UpdatedTrainer.Email && x.Id != id, ct);
        var PhoneExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == UpdatedTrainer.Phone && x.Id != id, ct);

        if (emailExists || PhoneExists)
            return false;

        mapper.Map(UpdatedTrainer,trainer);
        trainer.UpdatedAt = DateTime.Now;

         unitOfWork.GetRepository<Trainer>().Update(trainer);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0;
    }
}
