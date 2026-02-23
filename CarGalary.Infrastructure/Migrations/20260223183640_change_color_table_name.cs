using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_color_table_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarCarColors_CarColors_ColorId",
                table: "CarCarColors");

            migrationBuilder.DropTable(
                name: "CarColors");

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorNameAr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ColorNameEn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ColorNameAr",
                table: "Colors",
                column: "ColorNameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ColorNameEn",
                table: "Colors",
                column: "ColorNameEn",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarCarColors_Colors_ColorId",
                table: "CarCarColors",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarCarColors_Colors_ColorId",
                table: "CarCarColors");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.CreateTable(
                name: "CarColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorNameAr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ColorNameEn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarColors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarColors_ColorNameAr",
                table: "CarColors",
                column: "ColorNameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarColors_ColorNameEn",
                table: "CarColors",
                column: "ColorNameEn",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarCarColors_CarColors_ColorId",
                table: "CarCarColors",
                column: "ColorId",
                principalTable: "CarColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
