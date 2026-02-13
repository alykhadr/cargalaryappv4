

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarFeatureConfiguration
    : IEntityTypeConfiguration<CarFeature>
    {
        public void Configure(EntityTypeBuilder<CarFeature> builder)
        {

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.NameAr)
                   .IsRequired();
            builder.Property(x => x.NameEn)
                           .IsRequired();

            builder.Property(x => x.IsAvailable)
            .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
              .HasDefaultValueSql("GETUTCDATE()");

            // Unique constraint (no duplicate features like "GPS")
            builder.HasIndex(x => x.NameAr)
                   .IsUnique();
            builder.HasIndex(x => x.NameEn)
            .IsUnique();

            // Relationship
            builder.HasMany(x => x.CarCarFeatures)
                   .WithOne(x => x.CarFeature)
                   .HasForeignKey(x => x.CarFeatureId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}