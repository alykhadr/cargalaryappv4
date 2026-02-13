

using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
   public class CarGalleryImageConfiguration : IEntityTypeConfiguration<CarGalleryImage>
{
    public void Configure(EntityTypeBuilder<CarGalleryImage> builder)
    {

        builder.HasKey(c => c.ImageId);

        builder.Property(c => c.ImageUrl)
            .IsRequired();

        builder.Property(c => c.ImageType)
            .IsRequired();

        builder.Property(c => c.IsPrimary)
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        // Relationship: Car 1:N CarGalleryImage
        builder.HasOne(c => c.Car)
            .WithMany(c => c.CarImages)
            .HasForeignKey(c => c.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
}