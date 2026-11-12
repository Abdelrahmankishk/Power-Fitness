using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class TrainerService(IGenericRepository<Trainer> _TrainerRepository) : ITrainerService
{
    private readonly IGenericRepository<Trainer> trainerRepository = _TrainerRepository;

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
            specialtie = x.Specialtie
        });

        return TrainersViewModels;
    }
}
