
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
  

    public class CarExtraDetailsConfiguration : IEntityTypeConfiguration<CarExtraDetails>
    {
        public void Configure(EntityTypeBuilder<CarExtraDetails> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.NameAr)
                   .IsRequired();
                   builder.Property(e => e.NameEn)
                   .IsRequired();

            builder.Property(e => e.DescriptionEn);
               builder.Property(e => e.DescriptionAr);

            builder.Property(e => e.CreatedBy);
            builder.Property(e => e.CarExtraDetailsType);

            builder.Property(e => e.IsAvailable)
                   .HasDefaultValue(true);

            builder.HasOne(e => e.Car)
                   .WithMany(c => c.CarExtraDetails)
                   .HasForeignKey(e => e.CarId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}