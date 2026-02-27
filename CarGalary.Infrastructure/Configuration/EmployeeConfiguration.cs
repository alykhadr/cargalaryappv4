using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.EmployeeNo).IsUnique();
            builder.HasIndex(e => e.UserId).IsUnique();

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.BranchId).IsRequired();
            builder.Property(e => e.DepartmentId).IsRequired();

            builder.Property(e => e.EmployeeNo).IsRequired();
            builder.Property(e => e.NationalId).IsRequired();

            builder.Property(e => e.JobTitle).IsRequired();
            builder.Property(e => e.HireDate).IsRequired();
            builder.Property(e => e.EmploymentStatus).IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(e => e.IsAvailable)
                .HasDefaultValue(true);

            builder.HasOne(e => e.Branch)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
