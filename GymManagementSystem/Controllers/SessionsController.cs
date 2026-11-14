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
    private async Task RedirectWithData()
    {
        ViewBag.Trainers = new SelectList(await sessionService.GetTrainersFromSessionAsync(), "Id", "Name");
        ViewBag.Categories = new SelectList(await sessionService.GetCategoriesFromSessionAsync(), "Id", "CategoryName");
    }
    private readonly ISessionServices sessionService = _sessionService;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var Sessions = await sessionService.GetAllSessionsAsync(ct);
        if(!Sessions.success)
            TempData["ErrorMessage"] = Sessions.message;

        return View(Sessions.Value);
    }
    [HttpGet]
    public async Task<IActionResult> Details(int id , CancellationToken ct)
    {
        var session = await sessionService.GetSessionByIdAsync(id,ct);
        if(session.success)
            return View(session.Value);
        else
        {
            TempData["ErrorMessage"] = session.message;
            return RedirectToAction(nameof(Index));
        }
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

    #region Edit
    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var result = await sessionService.GetSessionToUpdateAsync(id, ct);
        if (result.success) {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainersFromSessionAsync(), "Id", "Name");
            return View(result.Value);
        }
        else
        {
            TempData["ErrorMessage"] = result.message;
            return RedirectToAction(nameof(Index));
        }
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateSessionViewModel updatedSession,CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainersFromSessionAsync(), "Id", "Name");
            return View(updatedSession);
        }
        var result = await sessionService.UpdateSessionAsync(id, updatedSession, ct);
        if (result.success)
        {
            TempData["SuccessMessage"] = "Session Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = result.message;
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainersFromSessionAsync(), "Id", "Name");
            return View(updatedSession);
        }
    }
    #endregion

}
