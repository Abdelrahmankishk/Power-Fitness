using GymManagementSystem.Models;


namespace GymManagementSystem.DAL.Repositories.Interfaces;

    public interface IPlanRepository : IGenericRepository<Plan>
    {
        Task<Plan> GetPlanWithMembers(int id,CancellationToken ct);

    }

