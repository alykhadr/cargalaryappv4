using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class remove_branchsid_from_cars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Branches_BranchsId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_BranchsId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "BranchsId",
                table: "Cars");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BranchId",
                table: "Cars",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Branches_BranchId",
                table: "Cars",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Branches_BranchId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_BranchId",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "BranchsId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BranchsId",
                table: "Cars",
                column: "BranchsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Branches_BranchsId",
                table: "Cars",
                column: "BranchsId",
                principalTable: "Branches",
                principalColumn: "Id");
        }
    }
}
