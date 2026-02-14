
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarCarFeatureConfiguration : IEntityTypeConfiguration<CarCarFeature>
    {
        public void Configure(EntityTypeBuilder<CarCarFeature> builder)
        {

            builder.HasKey(x => new { x.CarId, x.CarFeatureId });

            builder.HasOne(x => x.Car)
                   .WithMany(c => c.CarCarFeatures)
                   .HasForeignKey(x => x.CarId);

            builder.HasOne(x => x.CarFeature)
                   .WithMany(f => f.CarCarFeatures)
                   .HasForeignKey(x => x.CarFeatureId);
            builder.Property(x => x.IsAvailable)
        .HasDefaultValue(true);

            builder.Property(c => c.CarId);
            builder.Property(c => c.CarFeatureId);
            builder.Property(c => c.CreatedAt)
           .HasDefaultValueSql("GETUTCDATE()")  // Default to UTC now in SQL Server
           .ValueGeneratedOnAdd();
        }
    }

}