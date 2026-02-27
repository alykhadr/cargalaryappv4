using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260227201500_SeedCarInfoLookups")]
    public class SeedCarInfoLookups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CAR_VEHICLE_CLASS
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_VEHICLE_CLASS' AND DetailCode = '1')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_VEHICLE_CLASS', '1', N'اقتصادي', 'Economy', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_VEHICLE_CLASS' AND DetailCode = '2')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_VEHICLE_CLASS', '2', N'عادي', 'Standard', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_VEHICLE_CLASS' AND DetailCode = '3')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_VEHICLE_CLASS', '3', N'رياضي', 'Sport', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_VEHICLE_CLASS' AND DetailCode = '4')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_VEHICLE_CLASS', '4', N'فاخر', 'Luxury', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_VEHICLE_CLASS' AND DetailCode = '5')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_VEHICLE_CLASS', '5', N'دفع رباعي', 'SUV', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_VEHICLE_CLASS' AND DetailCode = '6')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_VEHICLE_CLASS', '6', N'بيك أب', 'Pickup', NULL, 'system', GETUTCDATE(), 1);");

            // CAR_TRANSMISION_TYPE
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_TRANSMISION_TYPE' AND DetailCode = '1')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_TRANSMISION_TYPE', '1', N'أوتوماتيك', 'AT', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_TRANSMISION_TYPE' AND DetailCode = '2')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_TRANSMISION_TYPE', '2', N'عادي', 'MT', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_TRANSMISION_TYPE' AND DetailCode = '3')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_TRANSMISION_TYPE', '3', N'ناقل متغير مستمر', 'CVT', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_TRANSMISION_TYPE' AND DetailCode = '4')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_TRANSMISION_TYPE', '4', N'ثنائي القابض', 'DCT', NULL, 'system', GETUTCDATE(), 1);");

            // CAR_DRIVETRAIN
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_DRIVETRAIN' AND DetailCode = '1')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_DRIVETRAIN', '1', N'دفع أمامي', 'FWD', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_DRIVETRAIN' AND DetailCode = '2')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_DRIVETRAIN', '2', N'دفع خلفي', 'RWD', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_DRIVETRAIN' AND DetailCode = '3')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_DRIVETRAIN', '3', N'دفع كلي', 'AWD', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_DRIVETRAIN' AND DetailCode = '4')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_DRIVETRAIN', '4', N'دفع رباعي', '4WD', NULL, 'system', GETUTCDATE(), 1);");

            // CAR_FUEL_TYPE
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_FUEL_TYPE' AND DetailCode = '1')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_FUEL_TYPE', '1', N'بنزين', 'Gasoline', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_FUEL_TYPE' AND DetailCode = '2')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_FUEL_TYPE', '2', N'ديزل', 'Diesel', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_FUEL_TYPE' AND DetailCode = '3')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_FUEL_TYPE', '3', N'هجين', 'Hybrid', NULL, 'system', GETUTCDATE(), 1);");
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_FUEL_TYPE' AND DetailCode = '4')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_FUEL_TYPE', '4', N'كهربائي', 'Electric', NULL, 'system', GETUTCDATE(), 1);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM LookupDetails WHERE MasterCode = 'CAR_VEHICLE_CLASS' AND DetailCode IN ('1','2','3','4','5','6');");
            migrationBuilder.Sql(@"DELETE FROM LookupDetails WHERE MasterCode = 'CAR_TRANSMISION_TYPE' AND DetailCode IN ('1','2','3','4');");
            migrationBuilder.Sql(@"DELETE FROM LookupDetails WHERE MasterCode = 'CAR_DRIVETRAIN' AND DetailCode IN ('1','2','3','4');");
            migrationBuilder.Sql(@"DELETE FROM LookupDetails WHERE MasterCode = 'CAR_FUEL_TYPE' AND DetailCode IN ('1','2','3','4');");
        }
    }
}
