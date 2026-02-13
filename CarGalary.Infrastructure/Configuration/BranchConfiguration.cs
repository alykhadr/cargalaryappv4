

using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{


    public class BranchConfiguration : IEntityTypeConfiguration<Branchs>
    {
        public void Configure(EntityTypeBuilder<Branchs> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.BranchNameAr)
                   .IsRequired();
                   builder.Property(b => b.BranchNameEn)
                   .IsRequired();

            builder.Property(b => b.DescriptionAr);
            builder.Property(b => b.DescriptionEn);

            builder.Property(b => b.MobileNo);

            builder.Property(b => b.WhatsUpNo);

            builder.Property(b => b.Email);

            builder.Property(b => b.Address);

            builder.Property(b => b.IsAvailable)
                   .HasDefaultValue(true);

            builder.Property(b => b.CreatedBy);

            builder.Property(b => b.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

                      builder.Property(b => b.Latitute);
                         builder.Property(b => b.Longtute);
        }
    }
}