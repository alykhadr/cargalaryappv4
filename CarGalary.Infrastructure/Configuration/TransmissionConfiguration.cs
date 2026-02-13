
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{

        public class TransmissionConfiguration : IEntityTypeConfiguration<Transmission>
    {
        public void Configure(EntityTypeBuilder<Transmission> builder)
        {
            builder.HasKey(e => e.Id);

             builder.Property(e => e.NameAr)
                    .IsRequired();
            builder.Property(e => e.NameEn)
           .IsRequired();

            builder.Property(e => e.DescriptionAr);
            builder.Property(e => e.DescriptionEn);

            builder.Property(e => e.CreatedBy);
             builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.IsAvailable)
                   .HasDefaultValue(true);

            builder.HasOne(e => e.Car)
                   .WithMany(c => c.Transmissions)
                   .HasForeignKey(e => e.CarId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    
}