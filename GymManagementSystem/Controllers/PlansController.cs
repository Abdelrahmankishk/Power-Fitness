using GymManagementSystem.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext dbContext;
        public PlansController()
        {
            dbContext = new GymDbContext();
        }
        public async Task<IActionResult> Index()
        {
            var plans = await dbContext.plans.ToListAsync();
            return View(plans);
        }
    }
}
