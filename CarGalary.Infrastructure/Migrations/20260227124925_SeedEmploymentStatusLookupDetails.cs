using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmploymentStatusLookupDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = 'Active')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('EMPLOYMENT_STATUS', 'Active', N'نشط', 'Active', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = 'OnLeave')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('EMPLOYMENT_STATUS', 'OnLeave', N'في إجازة', 'On Leave', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = 'Terminated')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('EMPLOYMENT_STATUS', 'Terminated', N'منتهي', 'Terminated', NULL, 'system', GETUTCDATE(), 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DELETE FROM LookupDetails
                  WHERE MasterCode = 'EMPLOYMENT_STATUS'
                    AND DetailCode IN ('Active', 'OnLeave', 'Terminated');");
        }
    }
}
