using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.BookingViewModels;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class BookingService(IUnitOfWork _unitOfWork, IMapper _mapper) : IBookingService
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly IMapper mapper = _mapper;

    public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
    {
        var session = await unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
        if (session is null) 
            return Result.NotFound("Session Not Found");

        if (session.StartDate <= DateTime.Now)
            return Result.Fail("Cannot Cancel this booking as if has already started");

        var booking = await unitOfWork.BookingRepository.FirstOrDefaultAsync(b => b.SessionId == sessionId && b.MemberId == memberId, tracking: true, ct: ct);
        if (booking is null) 
            return Result.NotFound("Booking Not Found");

        unitOfWork.BookingRepository.Delete(booking);
        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Booking Cancel Failed");
    }

    public async Task<Result> CreateNewBookingAsync(CreateBookingViewModel NewBooking, CancellationToken ct = default)
    {
        var session = await unitOfWork.SessionRepository.GetByIdAsync(NewBooking.SessionId, ct);
        if (session is null) 
            return Result.NotFound("Session Not Found");

        if (session.StartDate <= DateTime.Now)
            return Result.Fail("Cannot book this session as it has already started");

        var hasActiveMembership = await unitOfWork.MembershipRepository.AnyAsync(m => m.MemberId == NewBooking.MemberId && m.EndDate > DateTime.Now, ct);
        if (!hasActiveMembership)
            return Result.Fail("Member does not have an Active Membership.");

        var alreadyBooked = await unitOfWork.BookingRepository.AnyAsync(b => b.SessionId == NewBooking.SessionId && b.MemberId == NewBooking.MemberId, ct);
        if (alreadyBooked)
            return Result.Fail("Member already booked this Session");

        var booked = await unitOfWork.SessionRepository.GetCountOfBookedPlacesAsync(NewBooking.SessionId, ct);
        if (booked >= session.Capacity)
            return Result.Fail("Session is Full");

        unitOfWork.BookingRepository.Add(new Booking
        {
            MemberId = NewBooking.MemberId,
            SessionId = NewBooking.SessionId,
            IsAttended = false,
            CreatedAt = DateTime.Now,
        });

        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed To Book Session");
    }

    public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
    {
        var bookings = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);
        if (!bookings.Any()) 
            return null!;
        var MappedSession = mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(bookings);
        foreach (var item in MappedSession)
        {
            item.AvailableSlots = item.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedPlacesAsync(item.Id);
        }
        return MappedSession;
    }
    
    public async Task<IEnumerable<MembersSessionViewModel>> GetMembersForSelectAsync(int Id, CancellationToken ct = default)
    {
        var booking = await _unitOfWork.BookingRepository
                                             .GetAllAsync(x => x.SessionId == Id);

        var bookedMemberIds = booking.Select(x => x.MemberId).ToHashSet();

        var availableMembers = await _unitOfWork.GetRepository<Member>()
                                          .GetAllAsync(x => !bookedMemberIds.Contains(x.Id));

        return _mapper.Map<IEnumerable<MembersSessionViewModel>>(availableMembers);
    }


    public async Task<IEnumerable<MembersSessionViewModel>> GetMembersForOngoingByIdAsync(int Id, CancellationToken ct = default)
    {
        var bookings = await unitOfWork.BookingRepository.GetAllBySessionIdAsync(Id, ct);
        return bookings.Select(b => new MembersSessionViewModel
        {
            MemberId = b.MemberId,
            SessionId = Id,
            MemberName = b.Member.Name,
            BookingDate = b.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
            IsAttended = b.IsAttended,
        }).ToList();
    }

    public async Task<IEnumerable<MembersSessionViewModel>> GetMembersForUpcomingByIdAsync(int Id, CancellationToken ct = default)
    {
        var bookings = await unitOfWork.BookingRepository.GetAllBySessionIdAsync(Id, ct);
        var session = await unitOfWork.SessionRepository.GetByIdAsync(Id, ct: ct);
        return bookings.Select(b => new MembersSessionViewModel
        {
            MemberId = b.MemberId,
            SessionId = Id,
            MemberName = b.Member.Name,
            BookingDate = b.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
            IsAttended = session?.EndDate > DateTime.Now ? false : b.IsAttended
        }).ToList();
    }

    public async Task<Result> MarkAsAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
    {
        var booking = await unitOfWork.BookingRepository.FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, tracking: true, ct: ct);
        if (booking is null) return Result.NotFound("Booking Not Found");

        booking.IsAttended = true;
        booking.UpdatedAt = DateTime.Now;
        unitOfWork.BookingRepository.Update(booking);

        var result = await unitOfWork.SaveChangesAsync(ct);
        return result > 0 ? Result.OK() : Result.Fail("Failed to Mark As Attended");
    }
}
