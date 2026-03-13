using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestColorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE r
                SET r.ColorId = cc.ColorId
                FROM Requests r
                CROSS APPLY (
                    SELECT TOP (1) carColor.ColorId
                    FROM CarColors carColor
                    WHERE carColor.CarId = r.CarId
                      AND carColor.IsAvailable = 1
                    ORDER BY CASE WHEN ISNULL(carColor.StockQuantity, 0) > 0 THEN 0 ELSE 1 END, carColor.ColorId
                ) cc
                WHERE r.ColorId IS NULL;
                """);

            migrationBuilder.Sql(
                """
                UPDATE r
                SET r.ColorId = fallbackColor.Id
                FROM Requests r
                CROSS APPLY (
                    SELECT TOP (1) c.Id
                    FROM Colors c
                    WHERE c.IsAvailable = 1
                    ORDER BY c.Id
                ) fallbackColor
                WHERE r.ColorId IS NULL;
                """);

            migrationBuilder.AlterColumn<int>(
                name: "ColorId",
                table: "Requests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ColorId",
                table: "Requests",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Colors_ColorId",
                table: "Requests",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Colors_ColorId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ColorId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Requests");
        }
    }
}
