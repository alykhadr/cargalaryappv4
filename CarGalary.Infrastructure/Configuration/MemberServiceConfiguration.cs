

using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
   public class MemberServiceConfiguration : IEntityTypeConfiguration<MemberService>
{
    public void Configure(EntityTypeBuilder<MemberService> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(x => x.NameAr)
               .IsRequired();
               builder.Property(x => x.NameEn)
               .IsRequired();

        builder.Property(x => x.DescriptionAr);
        builder.Property(x => x.DescriptionEn);
        builder.Property(x => x.ImageUrl);

        builder.Property(x => x.IsAvailable)
               .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
               .IsRequired();
    }
}
}