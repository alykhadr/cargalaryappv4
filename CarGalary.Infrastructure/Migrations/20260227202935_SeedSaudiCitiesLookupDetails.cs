using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSaudiCitiesLookupDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '1001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '1001', N'الرياض', 'Riyadh', '1', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '1002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '1002', N'الخرج', 'Al Kharj', '1', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '1003')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '1003', N'الدوادمي', 'Al Dawadmi', '1', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '2001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '2001', N'مكة المكرمة', 'Makkah', '2', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '2002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '2002', N'جدة', 'Jeddah', '2', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '2003')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '2003', N'الطائف', 'Taif', '2', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '2004')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '2004', N'رابغ', 'Rabigh', '2', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '3001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '3001', N'المدينة المنورة', 'Madinah', '3', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '3002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '3002', N'ينبع', 'Yanbu', '3', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '3003')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '3003', N'العلا', 'AlUla', '3', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '4001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '4001', N'الدمام', 'Dammam', '4', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '4002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '4002', N'الخبر', 'Khobar', '4', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '4003')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '4003', N'الظهران', 'Dhahran', '4', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '4004')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '4004', N'الأحساء', 'Al Ahsa', '4', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '4005')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '4005', N'الجبيل', 'Jubail', '4', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '5001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '5001', N'بريدة', 'Buraidah', '5', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '5002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '5002', N'عنيزة', 'Unaizah', '5', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '6001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '6001', N'حائل', 'Hail', '6', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '7001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '7001', N'تبوك', 'Tabuk', '7', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '7002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '7002', N'ضباء', 'Duba', '7', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '8001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '8001', N'عرعر', 'Arar', '8', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '9001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '9001', N'جازان', 'Jazan', '9', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '9002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '9002', N'صبيا', 'Sabya', '9', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '10001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '10001', N'نجران', 'Najran', '10', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '11001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '11001', N'الباحة', 'Al Bahah', '11', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '12001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '12001', N'سكاكا', 'Sakaka', '12', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '12002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '12002', N'القريات', 'Al Qurayyat', '12', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '13001')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '13001', N'أبها', 'Abha', '13', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '13002')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '13002', N'خميس مشيط', 'Khamis Mushait', '13', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CITY' AND DetailCode = '13003')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CITY', '13003', N'بيشة', 'Bisha', '13', 'system', GETUTCDATE(), 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DELETE FROM LookupDetails
                  WHERE MasterCode = 'CITY'
                    AND DetailCode IN (
                      '1001','1002','1003',
                      '2001','2002','2003','2004',
                      '3001','3002','3003',
                      '4001','4002','4003','4004','4005',
                      '5001','5002',
                      '6001',
                      '7001','7002',
                      '8001',
                      '9001','9002',
                      '10001',
                      '11001',
                      '12001','12002',
                      '13001','13002','13003'
                    );");
        }
    }
}
