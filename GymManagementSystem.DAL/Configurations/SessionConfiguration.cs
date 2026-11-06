using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations;

internal class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.ToTable(s =>
        {
            s.HasCheckConstraint("CK_Session_Capacity", "[Capacity] between 1 and 25");

            s.HasCheckConstraint("CK_Session_Data", "EndDate > StartDate");
        });
    }
}
