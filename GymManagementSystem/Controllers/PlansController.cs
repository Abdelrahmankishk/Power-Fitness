using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    public class PlansController(IPlanRepository _planRepository) : Controller
    {
        private readonly IPlanRepository planRepository = _planRepository;
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planRepository.GetAllAsync(ct:ct);
            return View(plans);
        }
        public async Task<IActionResult> Details(int Id,CancellationToken ct) { 
            var plan = await planRepository.GetByIdAsync(Id,ct);
            if (plan == null) { 
                return RedirectToAction(nameof(Index));
            }
            else
                return View(plan);
        }
    }
}
