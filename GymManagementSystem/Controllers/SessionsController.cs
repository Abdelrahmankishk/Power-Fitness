using GymManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers;

public class SessionsController(ISessionServices _sessionService) : Controller
{
    private readonly ISessionServices sessionService = _sessionService;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var Sessions = await sessionService.GetAllSessionsAsync(ct);
        return View(Sessions);
    }
}
