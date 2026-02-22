using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <summary>
    /// Migration to add UpdatedAt and UpdatedBy audit fields to all entities inheriting from BaseEntity
    /// </summary>
    public partial class AddAuditFieldsToBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add UpdatedAt column to all tables that inherit from BaseEntity
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Branches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            // Add to other tables as needed
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Cars",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarBrands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CarBrands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarModels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CarModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CarTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarColors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CarColors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarFeatures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CarFeatures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ServiceTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ServiceTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Offers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FAQs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "FAQs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CompanyInformations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CompanyInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ContactUs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MemberServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "MemberServices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove UpdatedAt and UpdatedBy columns from all tables
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Branches");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "Branches");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Cars");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "Cars");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "CarBrands");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "CarBrands");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "CarModels");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "CarModels");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "CarTypes");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "CarTypes");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "CarColors");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "CarColors");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "CarFeatures");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "CarFeatures");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Services");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "Services");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "ServiceTypes");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "ServiceTypes");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Offers");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "Offers");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "FAQs");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "FAQs");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "CompanyInformations");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "CompanyInformations");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "ContactUs");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "ContactUs");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "MemberServices");
            migrationBuilder.DropColumn(name: "UpdatedBy", table: "MemberServices");
        }
    }
}
