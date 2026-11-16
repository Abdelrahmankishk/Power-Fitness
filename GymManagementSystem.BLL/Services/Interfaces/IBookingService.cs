using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.BookingViewModels;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
    Task<IEnumerable<MembersSessionViewModel>> GetMembersForUpcomingByIdAsync(int Id, CancellationToken ct = default);
    Task<IEnumerable<MembersSessionViewModel>> GetMembersForOngoingByIdAsync(int Id, CancellationToken ct = default);
    Task<IEnumerable<MembersSessionViewModel>> GetMembersForSelectAsync(int Id, CancellationToken ct = default);

    Task<Result> CreateNewBookingAsync(CreateBookingViewModel NewBooking, CancellationToken ct = default);
    Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default);
    Task<Result> MarkAsAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);

}
