using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class TrainerService(IGenericRepository<Trainer> _TrainerRepository,IGenericRepository<Session> _sessionRepository) : ITrainerService
{
    private readonly IGenericRepository<Trainer> trainerRepository = _TrainerRepository;
    private readonly IGenericRepository<Session> sessionRepository = _sessionRepository;

    public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default)
    {
        var emailExists = await trainerRepository.AnyAsync(x => x.Email == trainer.Email, ct);
        var PhoneExists = await trainerRepository.AnyAsync(x => x.Phone == trainer.Phone, ct);

        if (emailExists || PhoneExists)
            return false;

        var newTrainer = new Trainer()
        {
            Name = trainer.Name,
            Email = trainer.Email,
            Phone = trainer.Phone,
            Gender = trainer.Gender,
            DateOfBirth = trainer.DateOfBirth,
            Address = new()
            {
                Street = trainer.Street,
                BuildingNumber = trainer.BuildingNumber,
                City = trainer.City
            },
            Specialtie = trainer.Specialties
        };
        var result = await trainerRepository.AddAsync(newTrainer, ct);
        return result > 0 ? true : false;
    }

    public async Task<bool> DeleteTrainerAsync(int id, CancellationToken ct = default)
    {
        var trainer = await trainerRepository.GetByIdAsync(id,ct);
        if (trainer == null) 
            return false;
        var HasSessions = await sessionRepository.AnyAsync(s => s.TrainerId == id && s.StartDate > DateTime.Now);
        if(HasSessions)
            return false;

        var result = await trainerRepository.DeleteAsync(trainer, ct);
        return result > 0;
    }

    public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
    {
        var trainers = await trainerRepository.GetAllAsync(ct: ct);
        if (!trainers.Any())
            return [];

        var TrainersViewModels = trainers.Select(x => new TrainerViewModel()
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            Gender = x.Gender.ToString(),
            Phone = x.Phone,
            specialtie = x.Specialtie.ToString()
        });

        return TrainersViewModels;
    }

    public async Task<TrainerViewModel> GetTrainerByIdAsync(int id, CancellationToken ct = default)
    {
        var trainer = await trainerRepository.GetByIdAsync(id, ct);
        if (trainer == null)
            return null!;

        var trainerViewModel = new TrainerViewModel()
        {
            Name = trainer.Name,
            Email = trainer.Email,
            Gender = trainer.Gender.ToString(),
            DateofBirth = trainer.DateOfBirth.ToString(),
            Phone = trainer.Phone,
            specialtie = trainer.Specialtie.ToString(),
            Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
        };
        return trainerViewModel;
    }

    public async Task<UpdateTrainerViewModel> GetTrainerToUpdateAsync(int id, CancellationToken ct = default)
    {
        var trainer = await trainerRepository.GetByIdAsync(id, ct);
        if (trainer == null)
            return null!;
        else
            return new UpdateTrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                BuildingNumber = trainer.Address.BuildingNumber,
                specialtie = trainer.Specialtie
            };
    }

    public async Task<bool> UpdateTrainerDetailsAsync(int id, UpdateTrainerViewModel UpdatedTrainer, CancellationToken ct = default)
    {
        var trainer = await trainerRepository.GetByIdAsync(id, ct);
        if (trainer == null)
            return false;
        var emailExists = await trainerRepository.AnyAsync(x => x.Email == UpdatedTrainer.Email && x.Id != id, ct);
        var PhoneExists = await trainerRepository.AnyAsync(x => x.Phone == UpdatedTrainer.Phone && x.Id != id, ct);

        if (emailExists || PhoneExists)
            return false;

        trainer.Email = UpdatedTrainer.Email;
        trainer.Phone = UpdatedTrainer.Phone;
        trainer.Address.BuildingNumber = UpdatedTrainer.BuildingNumber;
        trainer.Address.Street = UpdatedTrainer.Street;
        trainer.Address.City = UpdatedTrainer.City;
        trainer.Specialtie = UpdatedTrainer.specialtie;
        trainer.UpdatedAt = DateTime.Now;
        var result = await trainerRepository.UpdateAsync(trainer,ct);
        return result > 0;
    }
}
