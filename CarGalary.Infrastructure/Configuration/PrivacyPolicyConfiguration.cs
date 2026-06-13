using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class PrivacyPolicyConfiguration : IEntityTypeConfiguration<PrivacyPolicy>
    {
        public void Configure(EntityTypeBuilder<PrivacyPolicy> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PrivacyPolicyAr)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(x => x.PrivacyPolicyEn)
                .HasColumnType("nvarchar(max)")
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
