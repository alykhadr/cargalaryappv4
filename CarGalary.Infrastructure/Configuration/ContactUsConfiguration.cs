

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
   public class ContactUsConfiguration : IEntityTypeConfiguration<ContactUs>
    {
        public void Configure(EntityTypeBuilder<ContactUs> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId);

            builder.Property(c => c.NameAr);
            builder.Property(c => c.NameEn);

            builder.Property(c => c.Email);

            builder.Property(c => c.MobileNo);

            builder.Property(c => c.MessageAr);
            builder.Property(c => c.MessageEn);
             builder.Property(c => c.IsAvailable).HasDefaultValue(true);

            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}