

using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class ServicesConfiguration : IEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
 

            builder.HasKey(x => x.Id);

           builder.Property(e => e.NameAr)
                   .IsRequired();
            builder.Property(e => e.NameEn)
           .IsRequired();

            builder.Property(e => e.DescriptionAr);
            builder.Property(e => e.DescriptionEn);

            builder.Property(x => x.ServiceTypeId);

             builder.Property(x => x.IsPercentage).HasDefaultValue(true);

            builder.Property(x => x.Discount)
                   .HasPrecision(5, 2);

            builder.Property(x => x.ServiceImageUrl);

            builder.Property(x => x.IsAvailable)
                   .IsRequired();
                    builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}