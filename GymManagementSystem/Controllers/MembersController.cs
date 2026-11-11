using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class MembersController(IGenericRepository<Member> _memberRepository) : Controller
    {
        private readonly IGenericRepository<Member> memberRepository = _memberRepository;

        // GET BaseURL/Members/Index
        // Index - List all members

        public async Task<IActionResult> Index()
        {
            var members = await memberRepository.GetAllAsync();
            return View(members);
        }

        // GET BaseURL/Members/MemberDetails/{id}
        // MemberDetails - Show one member's details

        // GET BaseURL/Members/HealthRecordDetails/{id}
        // HealthRecordDetails - Show one member's details

        #region Create Member
        // Get - BaseURL/Members/Create
        //Create- Show empty form

        // Post - BaseURL/Members/Create {Member}
        // CreateMember - Submits Form
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
