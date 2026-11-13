using GymManagementSystem.BLL.ViewModels.TrainerViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ITrainerService
{
    Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);

    Task<bool> CreateTrainerAsync(CreateTrainerViewModel trainer,CancellationToken ct = default);
    Task<TrainerViewModel> GetTrainerByIdAsync(int id,CancellationToken ct = default);
    Task<UpdateTrainerViewModel> GetTrainerToUpdateAsync(int id,CancellationToken ct = default);
    Task<bool> UpdateTrainerDetailsAsync(int id, UpdateTrainerViewModel UpdatedTrainer, CancellationToken ct = default);
    Task<bool> DeleteTrainerAsync(int id,CancellationToken ct = default);
}
