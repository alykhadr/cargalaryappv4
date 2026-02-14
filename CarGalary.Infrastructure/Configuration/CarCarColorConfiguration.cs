using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarCarColorConfiguration : IEntityTypeConfiguration<CarCarColor>
    {
        public void Configure(EntityTypeBuilder<CarCarColor> builder)
        {
            // Composite Key
            builder.HasKey(cc => new { cc.CarId, cc.ColorId });

            builder.Property(cc => cc.CarId);
            builder.Property(cc => cc.ColorId);
            // Relationships
            builder.HasOne(cc => cc.Car)
                .WithMany(c => c.CarColors)
                .HasForeignKey(cc => cc.CarId);

            builder.HasOne(cc => cc.CarColor)
                .WithMany(c => c.Cars)
                .HasForeignKey(cc => cc.ColorId);


            // prop
            builder.Property(cc => cc.StockQuantity);

            builder.Property(cc => cc.ColorImageUrl);
            builder.Property(c => c.PricingPerColor);
            builder.Property(b => b.CreatedAt)
           .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.IsAvailable).HasDefaultValue(true);
        }
    }
}
