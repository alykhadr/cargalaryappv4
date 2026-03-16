using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class PackagesConfiguration : IEntityTypeConfiguration<Packages>
    {
        public void Configure(EntityTypeBuilder<Packages> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.NameAr).IsRequired();
            builder.Property(p => p.NameEn).IsRequired();
            builder.Property(p => p.ImageUrl);
            builder.Property(p => p.CreatedBy);
            builder.Property(p => p.IsAvailable).HasDefaultValue(true);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
