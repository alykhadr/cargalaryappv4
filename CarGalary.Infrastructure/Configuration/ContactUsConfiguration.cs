

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
   public class ContactUsConfiguration : IEntityTypeConfiguration<ContactUs>
    {
        public void Configure(EntityTypeBuilder<ContactUs> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.ContactIconUrl);
            builder.Property(c => c.CreatedBy);

            builder.Property(c => c.ContactType);

            builder.Property(c => c.ContactValue);

            builder.Property(c => c.MessageAr);
            builder.Property(c => c.MessageEn);
             builder.Property(c => c.IsAvailable).HasDefaultValue(true);

            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}