using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveVatFromCarColorToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Vat",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(@"
                UPDATE c
                SET c.Vat = cc.Vat
                FROM Cars c
                CROSS APPLY (
                    SELECT TOP 1 Vat
                    FROM CarColors
                    WHERE CarId = c.Id
                    ORDER BY ColorId
                ) cc;");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "CarColors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Vat",
                table: "CarColors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(@"
                UPDATE cc
                SET cc.Vat = c.Vat
                FROM CarColors cc
                INNER JOIN Cars c ON c.Id = cc.CarId;");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "Cars");
        }
    }
}
