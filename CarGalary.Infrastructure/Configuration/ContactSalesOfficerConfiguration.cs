
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class ContactSalesOfficerConfiguration : IEntityTypeConfiguration<ContactSalesOfficer>
    {
        public void Configure(EntityTypeBuilder<ContactSalesOfficer> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ContactValue)
                   .IsRequired();

           builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.ContactType).IsRequired();
             builder.Property(e => e.ContactType);
 builder.Property(e => e.CreatedBy);
            builder.Property(e => e.IsAvailable)
                   .HasDefaultValue(true);

        }
    }
}