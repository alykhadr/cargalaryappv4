using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDashboardPeriodLookupDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'DASHBOARD_PERIOD' AND DetailCode = '1w')
    INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
    VALUES ('DASHBOARD_PERIOD', '1w', '1W', '1W', NULL, 'System', GETUTCDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'DASHBOARD_PERIOD' AND DetailCode = '2w')
    INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
    VALUES ('DASHBOARD_PERIOD', '2w', '2W', '2W', NULL, 'System', GETUTCDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'DASHBOARD_PERIOD' AND DetailCode = '1m')
    INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
    VALUES ('DASHBOARD_PERIOD', '1m', '1M', '1M', NULL, 'System', GETUTCDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'DASHBOARD_PERIOD' AND DetailCode = '2m')
    INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
    VALUES ('DASHBOARD_PERIOD', '2m', '2M', '2M', NULL, 'System', GETUTCDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'DASHBOARD_PERIOD' AND DetailCode = '3m')
    INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
    VALUES ('DASHBOARD_PERIOD', '3m', '3M', '3M', NULL, 'System', GETUTCDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'DASHBOARD_PERIOD' AND DetailCode = '6m')
    INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
    VALUES ('DASHBOARD_PERIOD', '6m', '6M', '6M', NULL, 'System', GETUTCDATE(), 1);

IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'DASHBOARD_PERIOD' AND DetailCode = '1y')
    INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
    VALUES ('DASHBOARD_PERIOD', '1y', '1Y', '1Y', NULL, 'System', GETUTCDATE(), 1);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM LookupDetails
WHERE MasterCode = 'DASHBOARD_PERIOD'
  AND DetailCode IN ('1w', '2w', '1m', '2m', '3m', '6m', '1y');
");
        }
    }
}
