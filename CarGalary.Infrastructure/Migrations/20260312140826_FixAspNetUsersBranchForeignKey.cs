using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAspNetUsersBranchForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('AspNetUsers', 'BranchsId') IS NOT NULL
BEGIN
    UPDATE U
    SET U.BranchId = U.BranchsId
    FROM AspNetUsers U
    WHERE (U.BranchId IS NULL OR U.BranchId = 0)
      AND U.BranchsId IS NOT NULL;

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_AspNetUsers_Branches_BranchsId')
        ALTER TABLE AspNetUsers DROP CONSTRAINT FK_AspNetUsers_Branches_BranchsId;

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AspNetUsers_BranchsId' AND object_id = OBJECT_ID('AspNetUsers'))
        DROP INDEX IX_AspNetUsers_BranchsId ON AspNetUsers;

    ALTER TABLE AspNetUsers DROP COLUMN BranchsId;
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AspNetUsers_BranchId' AND object_id = OBJECT_ID('AspNetUsers'))
    CREATE INDEX IX_AspNetUsers_BranchId ON AspNetUsers(BranchId);
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_AspNetUsers_Branches_BranchId')
BEGIN
    ALTER TABLE AspNetUsers
    ADD CONSTRAINT FK_AspNetUsers_Branches_BranchId
        FOREIGN KEY (BranchId) REFERENCES Branches(Id)
        ON DELETE NO ACTION;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Branches_BranchId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BranchId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "BranchsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

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
        }
    }
}
