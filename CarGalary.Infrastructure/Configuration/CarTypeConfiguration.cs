using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CarTypeConfiguration : IEntityTypeConfiguration<CarType>
{
    public void Configure(EntityTypeBuilder<CarType> builder)
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

        builder.HasMany(t => t.Cars)
            .WithOne(c => c.Type)
            .HasForeignKey(c => c.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

       
    }
}
