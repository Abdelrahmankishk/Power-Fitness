using System.Diagnostics;
using System.Threading.Tasks;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService _homeService)
        {
            _logger = logger;
            homeService = _homeService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var data = await homeService.GetAllAnalytics(ct);
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
