using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DbContexts
{
    public class GymDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=.;Database=PowerFitnessdb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public DbSet<Plan> plans { get; set; }
    }
}
