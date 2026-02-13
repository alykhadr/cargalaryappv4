
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            
            // Primary key
            builder.HasKey(up => up.Id);

            // One-to-one relationship with User
            builder.HasOne(up => up.User)
                   .WithOne(u => u.Profile)   // Add Profile navigation in User class
                   .HasForeignKey<UserProfile>(up => up.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Columns
            builder.Property(up => up.PhoneNumber);
              builder.Property(up => up.IsAvailable).HasDefaultValue(true);

            builder.Property(up => up.Address);

            builder.Property(up => up.ProfileImageUrl);

            builder.Property(up => up.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()"); // default UTC now

            builder.Property(up => up.CreatedBy);
        }
    }
}