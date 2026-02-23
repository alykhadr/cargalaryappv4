using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_contact_us_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "ContactUs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ContactUs",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "ContactUs",
                newName: "ContactValue");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "ContactUs",
                newName: "ContactIconUrl");

            migrationBuilder.AddColumn<int>(
                name: "ContactType",
                table: "ContactUs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactType",
                table: "ContactUs");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ContactUs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ContactValue",
                table: "ContactUs",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "ContactIconUrl",
                table: "ContactUs",
                newName: "NameAr");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
