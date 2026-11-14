using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Repositories.Classes;
using Microsoft.AspNetCore.Mvc;


namespace GymManagementSystem.Controllers
{
    public class PlansController(IPlanService _planService) : Controller
    {
        private readonly IPlanService planService = _planService;
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planService.GetAllPlansAsync(ct:ct);
            return View(plans.Value);
        }
        public async Task<IActionResult> Details(int Id, CancellationToken ct)
        {
            var plan = await planService.GetPlanByIdAsync(Id, ct);
            if (!plan.success)
            {
                TempData["ErrorMessage"] = plan.message;
                return RedirectToAction(nameof(Index));
            }
            else
                return View(plan.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Status(int Id, CancellationToken ct)
        {
            var StatusResult = await planService.ChangePlanStatus(Id, ct);
            if (StatusResult.success)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = StatusResult.message;

            return RedirectToActionPermanent(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditPlan(int Id, CancellationToken ct)
        {
            var plan = await planService.GetPlanToUpdateAsync(Id, ct);
            if (!plan.success)
            {
                TempData["ErrorMessage"] = plan.message;
                return RedirectToAction(nameof(Index));
            }
            return View(plan.Value);
        }
        [HttpPost]
        public async Task<IActionResult> EditPlan(int id, UpdatePlanViewModel UpdatedPlan, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(UpdatedPlan);

            var result= await planService.UpdatePlanDetailsAsync(id, UpdatedPlan, ct);
            if (result.success)
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            else
                TempData["ErrorMessage"] = result.message;

            return RedirectToActionPermanent(nameof(Index));
        }
    }
}
