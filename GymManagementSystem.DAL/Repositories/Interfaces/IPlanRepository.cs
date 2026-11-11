using GymManagementSystem.Models;


namespace GymManagementSystem.DAL.Repositories.Interfaces;

    public interface IPlanRepository : IGenericRepository<Plan>
    {
    Task<IEnumerable<Plan>> GetPlanWithMembers();

    }

