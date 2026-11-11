using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations;

public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(X => X.Phone)
              .HasColumnType("varchar")
              .HasMaxLength(11);

        builder.HasIndex(x => x.Phone)
            .IsUnique();

        builder.ToTable(gu =>
        {
            gu.HasCheckConstraint("Email Check", "[Email] LIKE '_%@_%._%'");
            gu.HasCheckConstraint("ValidPhoneNumber", " [Phone] LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
        });

        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.Street)
            .HasColumnName("Street")
                .HasColumnType("varchar")
                .HasMaxLength(30);
            address.Property(a => a.City)
                .HasColumnName("City")
                .HasColumnType("varchar")
                .HasMaxLength(30);
            address.Property(a => a.BuildingNumber)
                .HasColumnName("BuildingNumber")
                .HasColumnType("varchar")
                .HasMaxLength(10);
        });
    }
}
