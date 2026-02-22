using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ass_branch_id_fk_to_caar_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchsId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BranchsId",
                table: "Cars",
                column: "BranchsId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BranchsId",
                table: "AspNetUsers",
                column: "BranchsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Branches_BranchsId",
                table: "AspNetUsers",
                column: "BranchsId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Branches_BranchsId",
                table: "Cars",
                column: "BranchsId",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Branches_BranchsId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Branches_BranchsId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_BranchsId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BranchsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "BranchsId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchsId",
                table: "AspNetUsers");
        }
    }
}
