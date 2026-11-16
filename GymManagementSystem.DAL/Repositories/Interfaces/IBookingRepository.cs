using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IBookingRepository: IGenericRepository<Booking>
{
    public Task<List<Booking>> GetAllBySessionIdAsync(int Id, CancellationToken ct = default);
}
