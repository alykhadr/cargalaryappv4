using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class UserRateConfiguration : IEntityTypeConfiguration<UserRate>
    {
        public void Configure(EntityTypeBuilder<UserRate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ReviewerNameAr)
                .HasMaxLength(200);

            builder.Property(x => x.ReviewerNameEn)
                .HasMaxLength(200);

            builder.Property(x => x.CommentAr)
                .HasMaxLength(1000);

            builder.Property(x => x.CommentEn)
                .HasMaxLength(1000);

            builder.Property(x => x.RateValue)
                .IsRequired()
                .HasColumnType("decimal(3,2)");

            builder.Property(x => x.IsProductReview)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Car)
                .WithMany()
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => new { x.IsAvailable, x.IsProductReview, x.CreatedAt });

            builder.ToTable(t => t.HasCheckConstraint("CK_UserRates_RateValue", "[RateValue] >= 1 AND [RateValue] <= 5"));
        }
    }
}
