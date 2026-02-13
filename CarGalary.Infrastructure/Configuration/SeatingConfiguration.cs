

using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class SeatingConfiguration : IEntityTypeConfiguration<Seating>
    {
        public void Configure(EntityTypeBuilder<Seating> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.NameAr)
                   .IsRequired();
            builder.Property(e => e.NameEn)
           .IsRequired();

            builder.Property(e => e.DescriptionAr);
            builder.Property(e => e.DescriptionEn);

            builder.Property(e => e.CreatedBy);

            builder.Property(e => e.IsAvailable)
                   .HasDefaultValue(true);

            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne(e => e.Car)
                   .WithMany(c => c.Seatings)
                   .HasForeignKey(e => e.CarId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}