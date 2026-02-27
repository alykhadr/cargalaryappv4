using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.NameAr).IsRequired();
            builder.Property(d => d.NameEn).IsRequired();
            builder.Property(d => d.CreatedBy);
            builder.Property(d => d.IsAvailable).HasDefaultValue(true);
            builder.Property(d => d.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(d => d.NameAr).IsUnique();
            builder.HasIndex(d => d.NameEn).IsUnique();
        }
    }
}
