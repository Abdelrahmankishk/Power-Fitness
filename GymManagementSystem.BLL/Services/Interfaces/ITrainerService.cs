using GymManagementSystem.BLL.ViewModels.TrainerViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ITrainerService
{
    Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
}
