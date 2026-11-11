using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    public class PlansController(IGenericRepository<Plan> _planRepository) : Controller
    {
        private readonly IGenericRepository<Plan> planRepository = _planRepository;
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
