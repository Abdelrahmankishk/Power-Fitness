using GymManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace GymManagementSystem.Controllers
{
    public class PlansController(IPlanService _planService) : Controller
    {
        private readonly IPlanService planService = _planService;
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planService.GetAllPlansAsync(ct:ct);
            return View(plans);
        }
        public async Task<IActionResult> Details(int Id, CancellationToken ct)
        {
            var plan = await planService.GetPlanByIdAsync(Id, ct);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            else
                return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Status(int Id, CancellationToken ct)
        {
            var StatusResult = await planService.ChangePlanStatus(Id, ct);
            if (StatusResult)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed To Change Status";

            return RedirectToActionPermanent(nameof(Index));
        }
    }
}
