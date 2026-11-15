using GymManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GymManagementSystem.DAL.DataSeeder;

public static class IdentityDataSeeder
{
    public static async Task SeedIdentityAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct = default)
    {
        try
        {
            bool hasUser = await userManager.Users.AnyAsync(ct);
            bool hasRoles = await roleManager.Roles.AnyAsync(ct);

            if (hasUser && hasRoles)
                return;
            var roles = new List<IdentityRole>() {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin")
            };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    var roleResult = await roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        logger.LogError($"Failed to Create Role {role.Name}: {string.Join(" ; ", roleResult.Errors.Select(e => e.Description))})");
                        return;
                    }
                }
            }
            if (!hasUser)
            {
                var MainAdmin = new ApplicationUser
                {
                    FirstName = "Abdelrahman",
                    LastName = "Keshk",
                    Email = "abdelrahmankishk@gmail.com",
                    UserName = "AbdelrahmanKeshk",
                    PhoneNumber = "01014725368"
                };
                var userResult = await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                if (!userResult.Succeeded)
                {
                    logger.LogError($"Failed to Create Admin {MainAdmin.FirstName} {MainAdmin.LastName}: {string.Join(" ; ", userResult.Errors.Select(e => e.Description))})");
                    return;
                }
                await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                var Admin = new ApplicationUser
                {
                    FirstName = "Youssef",
                    LastName = "Keshk",
                    Email = "Youssef@gmail.com",
                    UserName = "YoussefKeshk",
                    PhoneNumber = "01029725368"
                };
                var userResult2 = await userManager.CreateAsync(Admin, "P@ssw0rd");
                if (!userResult2.Succeeded)
                {
                    logger.LogError($"Failed to Create Admin {Admin.FirstName} {Admin.LastName}: {string.Join(" ; ", userResult2.Errors.Select(e => e.Description))})");
                    return;
                }
                await userManager.AddToRoleAsync(Admin, "Admin");
                logger.LogInformation("Identity Data Seeded");
            }
            return;
        }
        catch (Exception ex) {
            logger.LogError(ex, $"Identity Seeding Failed");
            return;
        }
    }

}
