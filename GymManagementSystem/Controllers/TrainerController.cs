using GymManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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


        #region Create Trainer
        // Get - BaseURL/Trainers/Create
        //Create- Show empty form
        [HttpGet]
        public IActionResult Create() => View();

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
