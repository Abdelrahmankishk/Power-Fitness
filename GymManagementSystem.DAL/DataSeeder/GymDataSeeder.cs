using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GymManagementSystem.DAL.DataSeeder;

public static class GymDataSeeder
{
    public static async Task SeedAsync(GymDbContext context, string FilePath, ILogger logger, CancellationToken ct = default)
    {
        try
        {
            if (!await context.plans.AnyAsync(ct))
            {
                var plan = LoadDataFromJson<Plan>(FilePath,"plans.json");
                if (plan.Any())
                {
                    context.plans.AddRange(plan);
                    logger.LogInformation($"Plans Seeded: {plan.Count}");
                }
                if (context.ChangeTracker.HasChanges())
                    await context.SaveChangesAsync(ct);
                else
                    logger.LogInformation("Plans Already Seeded");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Data Seeding Failed");
            throw;
        }
    }
    private static List<T> LoadDataFromJson<T>(string filePath, string fileName)
    {
        var path = Path.Combine(filePath,fileName);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Seed Data File Not Foune: {path}");

        var data = File.ReadAllText(path);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
    }
}
