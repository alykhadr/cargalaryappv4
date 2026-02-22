using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class audit_fileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Transmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Transmissions",
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
                table: "Seatings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Seatings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Safeties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Safeties",
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
                table: "MemberServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "MemberServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Measurements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Measurements",
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
                table: "ExtraFeatures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ExtraFeatures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Exteriors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Exteriors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EngineSpecifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "EngineSpecifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EaseAndComforts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "EaseAndComforts",
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
                table: "ContactSalesOfficers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ContactSalesOfficers",
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
                table: "CarGalleryImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CarGalleryImages",
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
                table: "BranchWorkingDays",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "BranchWorkingDays",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AudioAndCommunicationSystems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AudioAndCommunicationSystems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Transmissions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Transmissions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Seatings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Seatings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Safeties");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Safeties");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MemberServices");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MemberServices");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FAQs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "FAQs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ExtraFeatures");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ExtraFeatures");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Exteriors");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Exteriors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EngineSpecifications");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EngineSpecifications");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EaseAndComforts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EaseAndComforts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ContactSalesOfficers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ContactSalesOfficers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CarTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarGalleryImages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CarGalleryImages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarFeatures");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CarFeatures");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CarColors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarBrands");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CarBrands");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BranchWorkingDays");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "BranchWorkingDays");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AudioAndCommunicationSystems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AudioAndCommunicationSystems");
        }
    }
}
