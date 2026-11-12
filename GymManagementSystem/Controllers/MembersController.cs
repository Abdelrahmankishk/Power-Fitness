using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers
{
    public class MembersController(IMemberService _memberService) : Controller
    {

        private readonly IMemberService memberService = _memberService;

        // GET BaseURL/Members/Index
        // Index - List all members

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await memberService.GetAllMemberAsync(ct);
            return View(members);
        }

        // GET BaseURL/Members/MemberDetails/{id}
        // MemberDetails - Show one member's details
        public async Task<IActionResult> MemberDetails(int id,CancellationToken ct)
        {
            var member = await memberService.GetMemberDetailsById(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToActionPermanent(nameof(Index));
            }

                return View(member);
        }

        // GET BaseURL/Members/HealthRecordDetails/{id}
        // HealthRecordDetails - Show one member's details
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await memberService.GetMemberHealthRecordAsync(id);
            if(healthRecord == null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToActionPermanent(nameof(Index));
            }
            return View(healthRecord);
                
        }

        #region Create Member
        // Get - BaseURL/Members/Create
        //Create- Show empty form
        [HttpGet]
        public IActionResult Create() => View();

        // Post - BaseURL/Members/Create {Member}
        // CreateMember - Submits Form
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

            var result = await memberService.CreateMemberAsync(model, ct);

            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member";

            return RedirectToActionPermanent(nameof(Index));
        }
        #endregion

        #region Edit Member
        // Get - BaseURL/Members/Edit/{id}
        //Edit - Diplay edit form

        // Post - BaseURL/Members/Edit {Member}
        // Edit - Submits Form
        #endregion

        #region Delete Member
        // Get - BaseURL/Members/Delete/{id}
        //Delete - Show Confermatiob Form

        // Post - BaseURL/Members/Delete {Member}
        // DeleteConfirmed - Submits Form
        #endregion
    }
}
