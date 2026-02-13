
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
    {
        public void Configure(EntityTypeBuilder<ServiceType> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(e => e.NameAr)
                    .IsRequired();
            builder.Property(e => e.NameEn)
           .IsRequired();

            builder.Property(e => e.DescriptionAr);
            builder.Property(e => e.DescriptionEn);


            builder.Property(x => x.IsAvailable).HasDefaultValue(true)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
             .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.HasMany(x => x.Services)
       .WithOne(x => x.ServiceType)
       .HasForeignKey(x => x.ServiceTypeId)
       .OnDelete(DeleteBehavior.Restrict);

        }
    }
}