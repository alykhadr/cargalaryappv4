using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(b => b.Id);
       builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(u => u.Email).IsUnique();

      

        builder.Property(u => u.Email)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

       

        builder.Property(u => u.IsAvailable)
            .HasDefaultValue(true);

            builder.Property(u => u.CreatedBy);
              builder.Property(u => u.Address);
                builder.Property(u => u.ProfileImageUrl);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.LastLoginAt);
        builder.Property(u => u.LastActivityAt);

        builder.HasOne(u => u.Branchs)
            .WithMany(b => b.ApplicationUsers)
            .HasForeignKey(u => u.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
       
    }
}
