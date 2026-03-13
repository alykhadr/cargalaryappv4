using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveColorStatusToCarColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorStatus",
                table: "CarColors",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                @"UPDATE cc
                  SET cc.ColorStatus = c.ColorStatus
                  FROM CarColors cc
                  INNER JOIN Colors c ON c.Id = cc.ColorId
                  WHERE cc.ColorStatus IS NULL;");

            migrationBuilder.Sql(
                @"UPDATE cc
                  SET cc.ColorStatus = s.Id
                  FROM CarColors cc
                  CROSS APPLY (
                      SELECT TOP (1) Id
                      FROM LookupDetails
                      WHERE MasterCode = 'CAR_COLOR_STATUS' AND DetailCode = 'Available' AND IsAvailable = 1
                      ORDER BY Id
                  ) s
                  WHERE cc.ColorStatus IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "ColorStatus",
                table: "CarColors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_Colors_LookupDetails_ColorStatus",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ColorStatus",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ColorStatus",
                table: "Colors");

            migrationBuilder.CreateIndex(
                name: "IX_CarColors_ColorStatus",
                table: "CarColors",
                column: "ColorStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_CarColors_LookupDetails_ColorStatus",
                table: "CarColors",
                column: "ColorStatus",
                principalTable: "LookupDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarColors_LookupDetails_ColorStatus",
                table: "CarColors");

            migrationBuilder.DropIndex(
                name: "IX_CarColors_ColorStatus",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "ColorStatus",
                table: "CarColors");

            migrationBuilder.AddColumn<int>(
                name: "ColorStatus",
                table: "Colors",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                @"UPDATE c
                  SET c.ColorStatus = x.ColorStatus
                  FROM Colors c
                  CROSS APPLY (
                      SELECT TOP (1) cc.ColorStatus
                      FROM CarColors cc
                      WHERE cc.ColorId = c.Id
                      ORDER BY cc.CreatedAt DESC
                  ) x
                  WHERE c.ColorStatus IS NULL;");

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
    }
}
