using GymManagementSystem.BLL.ViewModels.AccountViewModels;
using GymManagementSystem.Controllers;
using GymManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers;

public class AccountController(UserManager<ApplicationUser> _userManager,ILogger<AccountController> _logger,SignInManager<ApplicationUser> _signInManager) : Controller
{
    private readonly UserManager<ApplicationUser> userManager = _userManager;
    private readonly ILogger logger = _logger;
    private readonly SignInManager<ApplicationUser> signInManager = _signInManager;

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel AdminLogin, CancellationToken ct)
    {
        if(!ModelState.IsValid)
            return View(AdminLogin);

        var user = await userManager.FindByEmailAsync(AdminLogin.Email);
        if(user == null)
        {
            ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
            return View(AdminLogin);
        }
        var LoginResult = await signInManager.PasswordSignInAsync(user, AdminLogin.Password, AdminLogin.RememberMe, false);

        if (LoginResult.Succeeded)
        {
            logger.LogWarning($"User {user.UserName} Signed In");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        else if (LoginResult.IsLockedOut)
        {
            logger.LogWarning($"User {user.UserName} is Locked out");
            ModelState.AddModelError("InvalidLogin", "This Account is Locked Out, Try Again Later");
            return View(AdminLogin);
        }
        else
        {
            ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
            return View(AdminLogin);
        }
    }
}
