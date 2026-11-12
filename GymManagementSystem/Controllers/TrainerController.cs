using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers
{
    public class TrainerController(ITrainerService _trainerService) : Controller
    {
        private readonly ITrainerService trainerService = _trainerService;
        // Get BaseURL/Trainers/Index
        //Index - Lists all professional instructors
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        // Get BaseURL/Trainers/Details/{id}
        //Details - read-only profile for a single trainer.
        public async Task<IActionResult> Details(int id,CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerByIdAsync(id, ct);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            else
                return View(trainer);

        }

        #region Create Trainer
        // Get - BaseURL/Trainers/Create
        //Create- Show empty form
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel createTrainer,CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), createTrainer);

            var result = await trainerService.CreateTrainerAsync(createTrainer, ct);

            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member";

            return RedirectToAction(nameof(Index));
        }
        // Post - BaseURL/Trainers/Create {Trainer}
        // CreateMember - Submits Form
        #endregion

        #region Edit Trainer
        // Get - BaseURL/Trainers/Edit/{id}
        //Edit - Diplay edit form

        // Post - BaseURL/Trainers/Edit {Trainer}
        // Edit - Submits Form
        #endregion

        #region Delete Trainer
        // Get - BaseURL/Trainers/Delete/{id}
        //Delete - Show Confermatiob Form

        // Post - BaseURL/Trainers/Delete {Trainer}
        // DeleteConfirmed - Submits Form
        #endregion
    }
}
