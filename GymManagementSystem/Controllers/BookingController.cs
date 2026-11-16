using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers;
[Authorize]
public class BookingController(IBookingService _bookingService) : Controller
{
    public IBookingService BookingService { get; } = _bookingService;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var bookings = await BookingService.GetAllSessionsAsync(ct);
        return View(bookings);
    }

    #region Create
    [HttpGet]
    public async Task<IActionResult> Create(int id, CancellationToken ct)
    {
        var members = await BookingService.GetMembersForSelectAsync(id, ct);
        ViewBag.Members = new SelectList(
            members,
            nameof(MembersSessionViewModel.MemberId),
            nameof(MembersSessionViewModel.MemberName)
        );

        var model = new CreateBookingViewModel
        {
            SessionId = id
        };

        return View(model);

    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingViewModel newSession, CancellationToken ct)
    {
        var result = await BookingService.CreateNewBookingAsync(newSession, ct);
        TempData[result.success ? "SuccessMessage" : "ErrorMessage"] = result.success ? "Booking Created Successfully" : result.message;
        return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = newSession.SessionId });
    }
    #endregion
    [HttpGet]
    public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct) => View(await BookingService.GetMembersForUpcomingByIdAsync(id, ct));
    [HttpGet]
    public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken ct) => View(await BookingService.GetMembersForOngoingByIdAsync(id, ct));
    #region Attended
    [HttpPost]
    public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken ct)
    {
        var result = await BookingService.MarkAsAttendedAsync(memberId, sessionId, ct);
        TempData[result.success ? "SuccessMessage" : "ErrorMessage"] = result.success ? "Attendance Recorded Successfully" : result.message;
        return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
    }
    #endregion

    #region Cancel
    public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
    {
        var result = await BookingService.CancelBookingAsync(memberId, sessionId, ct);
        TempData[result.success ? "SuccessMessage" : "ErrorMessage"] = result.success ? "Booking Canceled Successfully" : result.message;
        return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
    }
    #endregion
}
