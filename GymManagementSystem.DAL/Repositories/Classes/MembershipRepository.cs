using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class MembershipRepository:GenericRepository<MemberShip>,IMembershipRepository
{
    private readonly GymDbContext dbContext;
    public MembershipRepository(GymDbContext _dbContext): base(_dbContext)
    {
        dbContext = _dbContext;
    }
    public async Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? condition = null,
           CancellationToken ct = default)
    {
        IQueryable<MemberShip> query = dbContext.MemberShips.AsNoTracking().Include(m => m.Plan).Include(m => m.Member);

        if (condition is not null) query = query.Where(condition);

        return await query.ToListAsync(ct);
    }
}
