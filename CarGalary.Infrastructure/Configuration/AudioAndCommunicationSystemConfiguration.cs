
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
  

    public class AudioAndCommunicationSystemConfiguration : IEntityTypeConfiguration<AudioAndCommunicationSystem>
    {
        public void Configure(EntityTypeBuilder<AudioAndCommunicationSystem> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.NameAr)
                   .IsRequired();
                   builder.Property(e => e.NameEn)
                   .IsRequired();

            builder.Property(e => e.Description);

            builder.Property(e => e.CreatedBy);

            builder.Property(e => e.IsAvailable)
                   .HasDefaultValue(true);

            builder.HasOne(e => e.Car)
                   .WithMany(c => c.AudioAndCommunicationSystems)
                   .HasForeignKey(e => e.CarId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}