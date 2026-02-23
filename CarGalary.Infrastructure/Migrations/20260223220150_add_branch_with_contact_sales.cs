using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_branch_with_contact_sales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ContactSalesOfficers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchsId",
                table: "ContactSalesOfficers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactSalesOfficers_BranchsId",
                table: "ContactSalesOfficers",
                column: "BranchsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactSalesOfficers_Branches_BranchsId",
                table: "ContactSalesOfficers",
                column: "BranchsId",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactSalesOfficers_Branches_BranchsId",
                table: "ContactSalesOfficers");

            migrationBuilder.DropIndex(
                name: "IX_ContactSalesOfficers_BranchsId",
                table: "ContactSalesOfficers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ContactSalesOfficers");

            migrationBuilder.DropColumn(
                name: "BranchsId",
                table: "ContactSalesOfficers");
        }
    }
}
