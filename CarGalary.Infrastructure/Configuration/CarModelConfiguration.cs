
using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalary.Infrastructure.Configuration
{
    public class CarModelConfiguration : IEntityTypeConfiguration<CarModel>
    {

        public void Configure(EntityTypeBuilder<CarModel> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(u => u.Id)
                 .ValueGeneratedOnAdd();

            builder.Property(t => t.NameAr)
                .IsRequired();
                 builder.Property(t => t.NameEn)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                                    .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.CreatedBy);
             builder.Property(c => c.IsAvailable).HasDefaultValue(true);
            builder.Property(c => c.ImageUrl);

            builder.HasMany(t => t.Cars)
                .WithOne(c => c.CarModel)
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
