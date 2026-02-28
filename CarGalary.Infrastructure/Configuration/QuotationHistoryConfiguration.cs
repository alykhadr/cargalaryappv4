using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class QuotationHistoryConfiguration : IEntityTypeConfiguration<QuotationHistory>
    {
        public void Configure(EntityTypeBuilder<QuotationHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.QuotationId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.StatusDate).IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.Property(x => x.IsAvailable)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Quotation)
                .WithMany(x => x.Histories)
                .HasForeignKey(x => x.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.StatusLookup)
                .WithMany()
                .HasForeignKey(x => x.Status)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
