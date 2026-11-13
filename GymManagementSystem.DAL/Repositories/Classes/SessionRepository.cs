using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    private readonly GymDbContext context;

    public SessionRepository(GymDbContext _context) : base(_context)
    {
        this.context = _context;
    }
    public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default)
    {
        var query = context.Sessions.AsNoTracking().Include(s=>s.Trainer).Include(s=>s.Category);
        return await query.ToListAsync(ct);
    }

    public async Task<int> GetCountOfBookedPlacesAsync(int sessionId, CancellationToken ct = default)
    {
        return await context.Bookings.AsNoTracking().CountAsync(book => book.SessionId == sessionId);
    }
}
