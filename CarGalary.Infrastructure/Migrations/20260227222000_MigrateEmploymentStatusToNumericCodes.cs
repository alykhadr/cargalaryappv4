using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260227222000_MigrateEmploymentStatusToNumericCodes")]
    public class MigrateEmploymentStatusToNumericCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Employees
                SET EmploymentStatus =
                    CASE
                        WHEN EmploymentStatus = 'Active' THEN '1'
                        WHEN EmploymentStatus = 'OnLeave' THEN '2'
                        WHEN EmploymentStatus = 'On Leave' THEN '2'
                        WHEN EmploymentStatus = 'Terminated' THEN '3'
                        ELSE EmploymentStatus
                    END
                WHERE EmploymentStatus IN ('Active', 'OnLeave', 'On Leave', 'Terminated');");

            migrationBuilder.Sql(@"
                DELETE FROM LookupDetails
                WHERE MasterCode = 'EMPLOYMENT_STATUS'
                  AND DetailCode IN ('Active', 'OnLeave', 'Terminated');");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = '1')
                INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                VALUES ('EMPLOYMENT_STATUS', '1', N'نشط', 'Active', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = '2')
                INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                VALUES ('EMPLOYMENT_STATUS', '2', N'في إجازة', 'On Leave', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = '3')
                INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                VALUES ('EMPLOYMENT_STATUS', '3', N'منتهي', 'Terminated', NULL, 'system', GETUTCDATE(), 1);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Employees
                SET EmploymentStatus =
                    CASE
                        WHEN EmploymentStatus = '1' THEN 'Active'
                        WHEN EmploymentStatus = '2' THEN 'OnLeave'
                        WHEN EmploymentStatus = '3' THEN 'Terminated'
                        ELSE EmploymentStatus
                    END
                WHERE EmploymentStatus IN ('1', '2', '3');");

            migrationBuilder.Sql(@"
                DELETE FROM LookupDetails
                WHERE MasterCode = 'EMPLOYMENT_STATUS'
                  AND DetailCode IN ('1', '2', '3');");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = 'Active')
                INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                VALUES ('EMPLOYMENT_STATUS', 'Active', N'نشط', 'Active', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = 'OnLeave')
                INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                VALUES ('EMPLOYMENT_STATUS', 'OnLeave', N'في إجازة', 'On Leave', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'EMPLOYMENT_STATUS' AND DetailCode = 'Terminated')
                INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                VALUES ('EMPLOYMENT_STATUS', 'Terminated', N'منتهي', 'Terminated', NULL, 'system', GETUTCDATE(), 1);");
        }
    }
}
