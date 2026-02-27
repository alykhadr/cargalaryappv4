using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarCarColorConfiguration : IEntityTypeConfiguration<CarColor>
    {
        public void Configure(EntityTypeBuilder<CarColor> builder)
        {
            // CarColor uses composite key, so BaseEntity.Id is intentionally ignored.
            builder.Ignore(cc => cc.Id);

            // Composite Key
            builder.HasKey(cc => new { cc.CarId, cc.ColorId });

            builder.Property(cc => cc.CarId);
            builder.Property(cc => cc.ColorId);
            // Relationships
            builder.HasOne(cc => cc.Car)
                .WithMany(c => c.CarColors)
                .HasForeignKey(cc => cc.CarId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Color)
                .WithMany(c => c.CarColors)
                .HasForeignKey(cc => cc.ColorId)
                  .OnDelete(DeleteBehavior.Cascade);


            // prop
            builder.Property(cc => cc.StockQuantity)
                .IsRequired();

            builder.Property(cc => cc.ColorImageUrl)
                .IsRequired();
            builder.Property(c => c.PricingPerColor)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(c => c.PricePefore)
                .HasColumnType("decimal(18,2)");
            builder.Property(c => c.VatAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(c => c.Discount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(c => c.DiscountType)
                .IsRequired();
            builder.Property(c => c.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.IsAvailable).HasDefaultValue(true);

            builder.ToTable(t => t.HasCheckConstraint("CK_CarColors_DiscountType", "[DiscountType] IN (0,1)"));
        }
    }
}
