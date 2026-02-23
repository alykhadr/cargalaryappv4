
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarCarFeatureConfiguration : IEntityTypeConfiguration<CarFeature>
    {
        public void Configure(EntityTypeBuilder<CarFeature> builder)
        {

            builder.HasKey(x => new { x.CarId, x.FeatureId });

            builder.HasOne(x => x.Car)
                   .WithMany(c => c.CarFeatures)
                   .HasForeignKey(x => x.CarId);

            builder.HasOne(x => x.Feature)
                   .WithMany(f => f.CarFeatures)
                   .HasForeignKey(x => x.FeatureId);
            builder.Property(x => x.IsAvailable)
        .HasDefaultValue(true);

            builder.Property(c => c.CarId);
            builder.Property(c => c.FeatureId);
            builder.Property(c => c.CreatedAt)
           .HasDefaultValueSql("GETUTCDATE()")  // Default to UTC now in SQL Server
           .ValueGeneratedOnAdd();
        }
    }

}