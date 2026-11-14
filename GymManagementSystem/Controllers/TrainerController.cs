using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
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
            return View(trainers.Value);
        }

        // Get BaseURL/Trainers/Details/{id}
        //Details - read-only profile for a single trainer.
        public async Task<IActionResult> Details(int id,CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerByIdAsync(id, ct);

            if (!trainer.success)
            {
                TempData["ErrorMessage"] = trainer.message;
                return RedirectToAction(nameof(Index));
            }
            else
                return View(trainer.Value);

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

            if (result.success)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.message;
            return View(nameof(Create), createTrainer);
        }
        // Post - BaseURL/Trainers/Create {Trainer}
        // CreateMember - Submits Form
        #endregion

        #region Edit Trainer
        // Get - BaseURL/Trainers/Edit/{id}
        //Edit - Diplay edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct) { 
            var trainer = await trainerService.GetTrainerToUpdateAsync(id, ct);
            if(!trainer.success)
            {
                TempData["ErrorMessage"] = trainer.message;
                return RedirectToActionPermanent(nameof(Index));
            }
            return View(trainer.Value);
        }

        // Post - BaseURL/Trainers/Edit {Trainer}
        // Edit - Submits Form
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateTrainerViewModel updatedTrainer, CancellationToken ct) {
            if (!ModelState.IsValid)
                return View(updatedTrainer);

            var result = await trainerService.UpdateTrainerDetailsAsync(id, updatedTrainer, ct);
            if (result.success)
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            else
                TempData["ErrorMessage"] = result.message;

            return RedirectToActionPermanent(nameof(Index));
        }
        #endregion

        #region Delete Trainer
        // Get - BaseURL/Trainers/Delete/{id}
        //Delete - Show Confermatiob Form]
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct) { 
            var trainer = await trainerService.GetTrainerByIdAsync(id,ct);
            if (!trainer.success)
            {
                TempData["ErrorMessage"] = trainer.message;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // Post - BaseURL/Trainers/Delete {Trainer}
        // DeleteConfirmed - Submits Form
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await trainerService.DeleteTrainerAsync(id,ct);
            if (result.success)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = result.message;

            return RedirectToActionPermanent(nameof(Index));
        }
        #endregion
    }
}
