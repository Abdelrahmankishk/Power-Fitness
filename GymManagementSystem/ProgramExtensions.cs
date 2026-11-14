using GymManagementSystem.DAL.DataSeeder;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.PL;

public static class ProgramExtensions
{
    public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        var pendingMigration = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigration.Any())
        {
            logger.LogInformation($"Applying {pendingMigration.Count()} Pending Migrations");
            await dbContext.Database.MigrateAsync();
        }
        //D:\Full Stack Diploma\Backend\08 MVC\GymManagementSolution\GymManagementSystem\wwwroot\Files\plans.json
        var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
        await GymDataSeeder.SeedAsync(dbContext, seedFolderPath, logger);
    }
}
