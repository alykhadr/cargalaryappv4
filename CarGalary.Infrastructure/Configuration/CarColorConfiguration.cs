
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ColorNameAr)
            .IsRequired();
        builder.Property(c => c.ColorNameEn)
            .IsRequired();
        builder.Property(c => c.ColorCode);

        builder.Property(c => c.IsAvailable)
            .IsRequired();

        builder.Property(c => c.CreatedBy);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        builder.HasIndex(c => c.ColorNameAr).IsUnique();
        builder.HasIndex(c => c.ColorNameEn).IsUnique();
    }
}
}
