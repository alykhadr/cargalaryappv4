using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class BranchWorkingDaysConfiguration : IEntityTypeConfiguration<BranchWorkingDays>
    {
        public void Configure(EntityTypeBuilder<BranchWorkingDays> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.DayAr)
                   .IsRequired();
                      builder.Property(b => b.DayEn)
                   .IsRequired();

            builder.Property(b => b.TimeType);
              builder.Property(b => b.WorkingFrom);
                builder.Property(b => b.WorkingTo);

            builder.Property(b => b.CreatedBy);

            builder.Property(b => b.IsAvailable)
                   .HasDefaultValue(true);

            builder.Property(b => b.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // Relationship with Branch
            builder.HasOne(b => b.Branch)
                   .WithMany(br => br.BranchWorkingDays)
                   .HasForeignKey(b => b.BranchId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}