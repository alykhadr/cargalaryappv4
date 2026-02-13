
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
     public class CompanyInformationConfiguration : IEntityTypeConfiguration<CompanyInformation>
    {
        public void Configure(EntityTypeBuilder<CompanyInformation> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CompanyNameAr)
                   .IsRequired();
                   builder.Property(c => c.CompanyNameEn)
                   .IsRequired();

            builder.Property(c => c.CRNumber)
                   .HasMaxLength(50);

            builder.Property(c => c.LogoUrl);

            builder.Property(c => c.MobileNo);

            builder.Property(c => c.TelNo);

            builder.Property(c => c.Email);

            builder.Property(c => c.AboutUsAr);
             builder.Property(c => c.AboutUsEn);

            builder.Property(c => c.OurMissionAr);
            builder.Property(c => c.OurMissionEn);

            builder.Property(c => c.OurGoalsAr);
                builder.Property(c => c.OurGoalsEn);
            builder.Property(c => c.IsAvailable).HasDefaultValue(true);
             builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}