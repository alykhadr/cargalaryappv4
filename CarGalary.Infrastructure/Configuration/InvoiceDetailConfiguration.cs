using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            builder.ToTable("InvoiceDetails");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
            builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);
            builder.Property(x => x.VatAmount).HasPrecision(18, 2);
            builder.Property(x => x.TotalAmount).HasPrecision(18, 2);

            builder.Property(x => x.Notes)
                .HasMaxLength(300);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(256);

            builder.Property(x => x.UpdatedBy)
                .HasMaxLength(256);

            builder.Property(x => x.IsAvailable)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.HasOne(x => x.Car)
                .WithMany()
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Color)
                .WithMany()
                .HasForeignKey(x => x.ColorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
