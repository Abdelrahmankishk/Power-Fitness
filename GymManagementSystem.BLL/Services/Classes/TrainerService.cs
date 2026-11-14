using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System.Collections.Generic;

namespace GymManagementSystem.BLL.Services.Classes;

public class TrainerService(IUnitOfWork _unitOfWork, IMapper _mapper) : ITrainerService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default)
    {
        var emailExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == trainer.Email, ct);
        var PhoneExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == trainer.Phone, ct);

        if (emailExists || PhoneExists)
            return Result.Validation("Email or Phone Already exits for other Trainer");

        var newTrainer = mapper.Map<Trainer>(trainer);
        unitOfWork.GetRepository<Trainer>().Add(newTrainer);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed to Create Trainer");
    }

    public async Task<Result> DeleteTrainerAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id,ct);
        if (trainer == null) 
            return Result.NotFound("Trainer Not Found");
        var HasSessions = await unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == id && s.StartDate > DateTime.Now);
        if(HasSessions)
            return Result.Validation("Trainer is Assigned to Upcoming Sessions");

        unitOfWork.GetRepository<Trainer>().Delete(trainer);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed to Delete Trainer");
    }

    public async Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken ct = default)
    {
        var trainers = await unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
        if (!trainers.Any())
            return Result<IEnumerable<TrainerViewModel>>.NotFound("NO Trainers Found");

        var TrainersViewModels = mapper.Map<IEnumerable<TrainerViewModel>>(trainers);

        return Result<IEnumerable<TrainerViewModel>>.OK( TrainersViewModels);
    }

    public async Task<Result<TrainerViewModel>> GetTrainerByIdAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
        if (trainer == null)
            return Result<TrainerViewModel>.NotFound("Trainer Not Found");

        var trainerViewModel = mapper.Map<TrainerViewModel>(trainer);
        return Result<TrainerViewModel>.OK( trainerViewModel);
    }

    public async Task<Result<UpdateTrainerViewModel>> GetTrainerToUpdateAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
        if (trainer == null)
            return Result<UpdateTrainerViewModel>.NotFound("Trainer Not Found");
        else
            return Result<UpdateTrainerViewModel>.OK( mapper.Map<UpdateTrainerViewModel>(trainer));
    }

    public async Task<Result> UpdateTrainerDetailsAsync(int id, UpdateTrainerViewModel UpdatedTrainer, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
        if (trainer == null)
            return Result.NotFound("Trainer Not Found");
        var emailExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == UpdatedTrainer.Email && x.Id != id, ct);
        var PhoneExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == UpdatedTrainer.Phone && x.Id != id, ct);

        if (emailExists || PhoneExists)
            return Result.Validation("Email or Phone Already exits for other user");

        mapper.Map(UpdatedTrainer,trainer);
        trainer.UpdatedAt = DateTime.Now;

         unitOfWork.GetRepository<Trainer>().Update(trainer);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed to Update Trainer");
    }
}
