using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations;

public class MemberShipConfigurations : IEntityTypeConfiguration<MemberShip>
{
    public void Configure(EntityTypeBuilder<MemberShip> builder)
    {
        builder.HasKey(ms => ms.Id);

        builder.Property(ms => ms.CreatedAt)
            .HasColumnName("StartDate")
            .HasDefaultValueSql("GETDATE()");

    }
}
