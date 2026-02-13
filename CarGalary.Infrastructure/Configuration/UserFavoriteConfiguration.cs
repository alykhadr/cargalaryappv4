
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class UserFavoriteConfiguration : IEntityTypeConfiguration<UserFavorite>
    {
        public void Configure(EntityTypeBuilder<UserFavorite> builder)
        {

            // Composite primary key
            builder.HasKey(uf => new { uf.UserId, uf.CarId });
            builder.HasOne(uf => uf.User)
                   .WithMany(u => u.Favorites)
                   .HasForeignKey(uf => uf.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uf => uf.Car)
                   .WithMany(c => c.FavoritedBy)
                   .HasForeignKey(uf => uf.CarId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.Notes);
            builder.Property(c => c.Priority);

            builder.Property(uf => uf.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}