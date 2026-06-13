using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CustomerPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CustomerEmail)
                .HasMaxLength(256);

            builder.Property(x => x.CustomerAddress)
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.Property(x => x.Subtotal).HasPrecision(18, 2);
            builder.Property(x => x.VatTotal).HasPrecision(18, 2);
            builder.Property(x => x.ShippingFee).HasPrecision(18, 2);
            builder.Property(x => x.ExtraDiscount).HasPrecision(18, 2);
            builder.Property(x => x.GrandTotal).HasPrecision(18, 2);

            builder.Property(x => x.IsAvailable)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.HasIndex(x => x.InvoiceNumber)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentMethodLookup)
                .WithMany()
                .HasForeignKey(x => x.PaymentMethod)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Details)
                .WithOne(x => x.Invoice)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
