using CarGalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(b => b.Id);
       builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Mileage);
        builder.Property(c => c.CreatedBy);

        builder.Property(c => c.DescriptionAr);
        builder.Property(c => c.DescriptionEn);
        builder.Property(c => c.NameAr).IsRequired();
        builder.Property(c => c.NameEn).IsRequired();
        builder.Property(c => c.Vat).IsRequired().HasDefaultValue(15m);
        builder.Property(c => c.ConditionId).IsRequired();
        builder.Property(c => c.SeatingCapacity).IsRequired();
        builder.Property(c => c.WeelSizeInch).IsRequired();
        builder.Property(c => c.FuelTankCapacityLiter).IsRequired();
        builder.Property(c => c.TrimLevel).IsRequired();
        builder.Property(c => c.VehicleClass).IsRequired();
        builder.Property(c => c.PlateNumberAr);
        builder.Property(c => c.PlateNumberEn);
        builder.Property(c => c.TransmisionType).IsRequired();
        builder.Property(c => c.Drivetrain).IsRequired();
        builder.Property(c => c.Cylenders).IsRequired();
        builder.Property(c => c.FuelType).IsRequired();
        builder.Property(c => c.ManufactureCountryId);
        builder.Property(c => c.EnginNumber).IsRequired();

        builder.Property(c => c.IsAvailable)
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(c => c.Branchs)
            .WithMany(b => b.Cars)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
