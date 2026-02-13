

using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{

public class FAQConfiguration : IEntityTypeConfiguration<FAQ>
{
    public void Configure(EntityTypeBuilder<FAQ> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TitleAr)
               .IsRequired();
                builder.Property(x => x.TitleEn)
               .IsRequired();

        builder.Property(x => x.DescriptionAr)
               .IsRequired();
                 builder.Property(x => x.DescriptionEn)
               .IsRequired();

        builder.Property(x => x.Order)
               .IsRequired();

        builder.Property(x => x.IsAvailable)
        .HasDefaultValue(true)
               .IsRequired();

        builder.Property(x => x.CreatedAt)
          .HasDefaultValueSql("GETUTCDATE()")
               .IsRequired();
    }
}

}