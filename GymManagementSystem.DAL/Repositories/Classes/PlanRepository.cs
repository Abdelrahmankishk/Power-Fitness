
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class PlanRepository : GenericRepository<Plan>, IPlanRepository
{
    public PlanRepository(GymDbContext _context):base(_context)
    {

    }

    public async Task<Plan> GetPlanWithMembers(int id, CancellationToken ct)
    {
        var plan = await dbSet.Include(p=>p.Memberships)
            .FirstOrDefaultAsync(x => x.Id == id,ct);

        return plan!;
    }
}

