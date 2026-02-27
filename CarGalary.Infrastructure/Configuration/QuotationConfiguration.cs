using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
    {
        public void Configure(EntityTypeBuilder<Quotation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.MobileNo)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.Property(x => x.VehicleOwnerType).IsRequired();
            builder.Property(x => x.PaymentMethod).IsRequired();
            builder.Property(x => x.RegionId).IsRequired();
            builder.Property(x => x.CityId).IsRequired();
            builder.Property(x => x.CarId).IsRequired();

            builder.Property(x => x.IsAvailable)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(x => x.UserId)
                .IsUnique()
                .HasFilter("[UserId] IS NOT NULL");

            builder.HasOne(x => x.User)
                .WithOne(u => u.Quotation)
                .HasForeignKey<Quotation>(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Car)
                .WithMany()
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentMethodLookup)
                .WithMany()
                .HasForeignKey(x => x.PaymentMethod)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.VehicleOwnerTypeLookup)
                .WithMany()
                .HasForeignKey(x => x.VehicleOwnerType)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RegionLookup)
                .WithMany()
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CityLookup)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
