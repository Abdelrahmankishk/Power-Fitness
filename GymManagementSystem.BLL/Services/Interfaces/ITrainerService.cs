using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ITrainerService
{
    Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken ct = default);
    Task<Result> CreateTrainerAsync(CreateTrainerViewModel trainer,CancellationToken ct = default);
    Task<Result<TrainerViewModel>> GetTrainerByIdAsync(int id,CancellationToken ct = default);
    Task<Result<UpdateTrainerViewModel>> GetTrainerToUpdateAsync(int id,CancellationToken ct = default);
    Task<Result> UpdateTrainerDetailsAsync(int id, UpdateTrainerViewModel UpdatedTrainer, CancellationToken ct = default);
    Task<Result> DeleteTrainerAsync(int id,CancellationToken ct = default);
}
