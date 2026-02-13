using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(b => b.Id);
       builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Color);
          builder.Property(c => c.Mileage);
            builder.Property(c => c.CreatedBy);
          

        builder.Property(c => c.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.DescriptionAr);
        builder.Property(c => c.DescriptionEn);

        builder.Property(c => c.IsAvailable)
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
