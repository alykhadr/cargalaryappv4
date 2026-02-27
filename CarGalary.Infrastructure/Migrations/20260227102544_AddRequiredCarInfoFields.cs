using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredCarInfoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConditionId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "FuelTankCapacityLiter",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SeatingCapacity",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrimLevel",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleClass",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WeelSizeInch",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConditionId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "FuelTankCapacityLiter",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "SeatingCapacity",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TrimLevel",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "VehicleClass",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "WeelSizeInch",
                table: "Cars");
        }
    }
}
