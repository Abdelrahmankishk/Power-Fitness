using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    private readonly GymDbContext context;

    public BookingRepository(GymDbContext _context): base(_context)
    {
        context = _context;
    }
    public Task<List<Booking>> GetAllBySessionIdAsync(int Id, CancellationToken ct = default)
    {
        return context.Bookings.AsNoTracking().Include(b => b.Member).Where(b => b.SessionId == Id).ToListAsync(ct);
    }
}
