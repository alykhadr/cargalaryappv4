using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class LookupDetailsConfiguration : IEntityTypeConfiguration<LookupDetails>
    {
        public void Configure(EntityTypeBuilder<LookupDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.MasterCode).IsRequired();
            builder.Property(x => x.DetailCode).IsRequired();
            builder.Property(x => x.NameAr).IsRequired();
            builder.Property(x => x.NameEn).IsRequired();
            builder.Property(x => x.MappedCode);
            builder.Property(x => x.CreatedBy);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.IsAvailable)
                .HasDefaultValue(true);

            builder.HasIndex(x => new { x.MasterCode, x.DetailCode }).IsUnique();
        }
    }
}
