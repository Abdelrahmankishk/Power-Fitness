using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class TrainerService(IGenericRepository<Trainer> _TrainerRepository) : ITrainerService
{
    private readonly IGenericRepository<Trainer> trainerRepository = _TrainerRepository;
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
