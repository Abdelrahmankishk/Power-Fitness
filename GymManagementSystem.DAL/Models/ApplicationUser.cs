using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.DAL.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
