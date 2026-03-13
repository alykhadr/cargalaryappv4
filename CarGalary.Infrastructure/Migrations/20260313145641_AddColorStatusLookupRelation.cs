using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColorStatusLookupRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorStatus",
                table: "Colors",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_COLOR_STATUS' AND DetailCode = 'Available')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_COLOR_STATUS', 'Available', N'متاح', 'Available', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_COLOR_STATUS' AND DetailCode = 'Reserved')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_COLOR_STATUS', 'Reserved', N'محجوز', 'Reserved', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_COLOR_STATUS' AND DetailCode = 'PendingSale')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_COLOR_STATUS', 'PendingSale', N'قيد البيع', 'Pending Sale', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_COLOR_STATUS' AND DetailCode = 'Sold')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_COLOR_STATUS', 'Sold', N'مباع', 'Sold', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'CAR_COLOR_STATUS' AND DetailCode = 'Inactive')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('CAR_COLOR_STATUS', 'Inactive', N'غير نشط', 'Inactive', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"UPDATE c
                  SET c.ColorStatus = s.Id
                  FROM Colors c
                  CROSS APPLY (
                      SELECT TOP (1) Id
                      FROM LookupDetails
                      WHERE MasterCode = 'CAR_COLOR_STATUS' AND DetailCode = 'Available' AND IsAvailable = 1
                      ORDER BY Id
                  ) s
                  WHERE c.ColorStatus IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "ColorStatus",
                table: "Colors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ColorStatus",
                table: "Colors",
                column: "ColorStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_LookupDetails_ColorStatus",
                table: "Colors",
                column: "ColorStatus",
                principalTable: "LookupDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_LookupDetails_ColorStatus",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ColorStatus",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ColorStatus",
                table: "Colors");

            migrationBuilder.Sql(
                @"DELETE FROM LookupDetails
                  WHERE MasterCode = 'CAR_COLOR_STATUS'
                    AND DetailCode IN ('Available', 'Reserved', 'PendingSale', 'Sold', 'Inactive');");
        }
    }
}
