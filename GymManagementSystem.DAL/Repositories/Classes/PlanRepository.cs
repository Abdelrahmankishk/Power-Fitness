
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class PlanRepository(GymDbContext _context) : IPlanRepository
{
    private readonly GymDbContext context = _context;

    public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
    {
        context.plans.Add(plan);
        return await context.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
    {
        context.plans.Remove(plan);
        return await context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
    {
        IQueryable<Plan> query = tracking ? context.plans : context.plans.AsNoTracking();
        return await query.ToListAsync(ct);
    }

    public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await context.plans.FindAsync(id, ct);
    }

    public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
    {
        context.plans.Update(plan);
        return await context.SaveChangesAsync(ct);
    }
}

