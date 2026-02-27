using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePlateNumberWithArEn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlateNumber",
                table: "Cars",
                newName: "PlateNumberEn");

            migrationBuilder.AddColumn<string>(
                name: "PlateNumberAr",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlateNumberAr",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "PlateNumberEn",
                table: "Cars",
                newName: "PlateNumber");
        }
    }
}
