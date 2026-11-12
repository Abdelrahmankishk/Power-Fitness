using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        //public async Task<IActionResult> Details(int Id,CancellationToken ct) { 
        //    var plan = await planRepository.GetByIdAsync(Id,ct);
        //    if (plan == null) { 
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //        return View(plan);
        //}
    }
}
