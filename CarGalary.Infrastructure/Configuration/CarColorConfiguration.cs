
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarColorConfiguration : IEntityTypeConfiguration<CarColor>
{
    public void Configure(EntityTypeBuilder<CarColor> builder)
    {
        // Table name (optional, defaults to DbSet name)

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.ColorNameAr)
            .IsRequired() ;               // NOT NULL          // Max length 50
  builder.Property(c => c.ColorNameEn)
            .IsRequired() ;     
        builder.Property(c => c.ColorCode);          // Optional, max length 20

        builder.Property(c => c.IsAvailable)
            .IsRequired();               // NOT NULL

        builder.Property(c => c.CreatedBy);          // Optional, max length 50

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")  // Default to UTC now in SQL Server
            .ValueGeneratedOnAdd();

        // Optional: add unique constraint on ColorName if needed
        builder.HasIndex(c => c.ColorNameAr).IsUnique();
         builder.HasIndex(c => c.ColorNameEn).IsUnique();
    }
}
}