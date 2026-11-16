using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers;

public class MembershipController(IMembershipService _membershipService) : Controller
{
    private readonly IMembershipService membershipService = _membershipService;

    [Authorize]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var Memberships = await membershipService.GetAllMembershipsAsync(ct);
        return View(Memberships);
    }

    #region Create
    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await RedirectWithData(ct);
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberShipViewModel NewMembersip, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await RedirectWithData(ct);
            return View(NewMembersip);
        }

        var result = await _membershipService.CreateMembershipAsync(NewMembersip, ct);
        if (result.success)
        {
            TempData["SuccessMessage"] = "Membership created successfully.";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.message;
        await RedirectWithData(ct);
        return View(NewMembersip);
    }
    #endregion

    #region Cancel
    [HttpPost]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        var result = await _membershipService.DeleteActiveMembershipAsync(id, ct);
        TempData[result.success ? "SuccessMessage" : "ErrorMessage"] =
            result.success ? "Membership Cancelled Successfully" : result.message;
        return RedirectToAction(nameof(Index));
    }
    #endregion


    private async Task RedirectWithData(CancellationToken ct)
    {
        ViewBag.Plans = new SelectList(await _membershipService.GetPlansForSelectAsync(ct), "Id", "Name");
        ViewBag.Members = new SelectList(await _membershipService.GetMembersForSelectAsync(ct), "Id", "Name");
    }
}
