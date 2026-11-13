using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers;

public class SessionsController(ISessionServices _sessionService) : Controller
{
    private readonly ISessionServices sessionService = _sessionService;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var Sessions = await sessionService.GetAllSessionsAsync(ct);
        if(!Sessions.success)
            TempData["ErrorMessage"] = Sessions.message;

        return View(Sessions.Value);
    }

    #region Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await RedirectWithData();
        return View();
    }
        [HttpPost]
    public async Task<IActionResult> Create(CreateSessionViewModel newSession,CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(newSession);

        var result= await sessionService.CreateSessionAsync(newSession);
        if (result.success)
        {
            TempData["SuccessMessage"] = "Session Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.message;
        await RedirectWithData();
        return View(newSession);
    }
    #endregion

    private async Task RedirectWithData()
    {
        ViewBag.Trainers = new SelectList(await sessionService.GetTrainersFromSessionAsync(), "Id", "Name");
        ViewBag.Categories = new SelectList(await sessionService.GetCategoriesFromSessionAsync(), "Id", "CategoryName");
    }
}
