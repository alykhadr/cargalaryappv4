
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OfferNameAr)
                   .IsRequired();
                        builder.Property(o => o.OfferNameEn)
                   .IsRequired();

            builder.Property(o => o.DescriptionAr);
            builder.Property(o => o.DescriptionEn);
            builder.Property(o => o.ExpiredAt);

            builder.Property(o => o.OfferImageUrl);

            builder.Property(o => o.IsAvailable)
                   .HasDefaultValue(true);

            builder.Property(o => o.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}