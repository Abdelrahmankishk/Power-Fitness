
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class PlanRepository : GenericRepository<Plan>,IPlanRepository
{
    public PlanRepository(GymDbContext _context):base(_context)
    {

    }


    public Task<IEnumerable<Plan>> GetPlanWithMembers()
    {
        throw new NotImplementedException();
    }


}

