using GymManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}
