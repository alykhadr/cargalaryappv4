
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarCarFeatureConfiguration: IEntityTypeConfiguration<CarCarFeature>
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
    }
}

}