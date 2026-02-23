using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_ContactType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMobileNo",
                table: "ContactSalesOfficers");

            migrationBuilder.AddColumn<string>(
                name: "ContactIconUrl",
                table: "ContactSalesOfficers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactType",
                table: "ContactSalesOfficers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactIconUrl",
                table: "ContactSalesOfficers");

            migrationBuilder.DropColumn(
                name: "ContactType",
                table: "ContactSalesOfficers");

            migrationBuilder.AddColumn<bool>(
                name: "IsMobileNo",
                table: "ContactSalesOfficers",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
