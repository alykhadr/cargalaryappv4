using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCarColorPricingRequiredFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "CarColors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PricingPerColor",
                table: "CarColors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ColorImageUrl",
                table: "CarColors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "CarColors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "CarColors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePefore",
                table: "CarColors",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "CarColors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Vat",
                table: "CarColors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatAmount",
                table: "CarColors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "CK_CarColors_DiscountType",
                table: "CarColors",
                sql: "[DiscountType] IN (0,1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_CarColors_DiscountType",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "PricePefore",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "VatAmount",
                table: "CarColors");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "CarColors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricingPerColor",
                table: "CarColors",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "ColorImageUrl",
                table: "CarColors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
