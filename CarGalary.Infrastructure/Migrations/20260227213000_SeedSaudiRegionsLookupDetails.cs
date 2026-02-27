using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260227213000_SeedSaudiRegionsLookupDetails")]
    public class SeedSaudiRegionsLookupDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '1')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '1', N'الرياض', 'Riyadh', 'SA-01', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '2')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '2', N'مكة المكرمة', 'Makkah', 'SA-02', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '3')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '3', N'المدينة المنورة', 'Madinah', 'SA-03', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '4')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '4', N'المنطقة الشرقية', 'Eastern Province', 'SA-04', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '5')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '5', N'القصيم', 'Al-Qassim', 'SA-05', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '6')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '6', N'حائل', 'Hail', 'SA-06', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '7')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '7', N'تبوك', 'Tabuk', 'SA-07', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '8')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '8', N'الحدود الشمالية', 'Northern Borders', 'SA-08', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '9')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '9', N'جازان', 'Jazan', 'SA-09', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '10')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '10', N'نجران', 'Najran', 'SA-10', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '11')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '11', N'الباحة', 'Al Bahah', 'SA-11', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '12')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '12', N'الجوف', 'Al Jawf', 'SA-12', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REGION' AND DetailCode = '13')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REGION', '13', N'عسير', 'Asir', 'SA-14', 'system', GETUTCDATE(), 1);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DELETE FROM LookupDetails
                  WHERE MasterCode = 'REGION'
                    AND DetailCode IN ('1','2','3','4','5','6','7','8','9','10','11','12','13');");
        }
    }
}
