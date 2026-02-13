using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CarBrandConfiguration : IEntityTypeConfiguration<CarBrand>
{
    public void Configure(EntityTypeBuilder<CarBrand> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();



        builder.Property(b => b.NameAr)
            .IsRequired();

        builder.Property(b => b.NameEn)
                   .IsRequired();

        builder.Property(b => b.ImageUrl)
       .IsRequired();

        builder.Property(c => c.CreatedBy);
        builder.Property(c => c.IsAvailable).HasDefaultValue(true);

        builder.Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(b => b.CarModels)
            .WithOne(c => c.Brand)
            .HasForeignKey(c => c.BrandId)
            .OnDelete(DeleteBehavior.Restrict);




    }
}
