using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_car_feature_table_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarCarFeatures_CarFeatures_CarFeatureId",
                table: "CarCarFeatures");

            migrationBuilder.DropTable(
                name: "CarFeatures");

            migrationBuilder.RenameColumn(
                name: "CarFeatureId",
                table: "CarCarFeatures",
                newName: "FeatureId");

            migrationBuilder.RenameIndex(
                name: "IX_CarCarFeatures_CarFeatureId",
                table: "CarCarFeatures",
                newName: "IX_CarCarFeatures_FeatureId");

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_NameAr",
                table: "Features",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_NameEn",
                table: "Features",
                column: "NameEn",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarCarFeatures_Features_FeatureId",
                table: "CarCarFeatures",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarCarFeatures_Features_FeatureId",
                table: "CarCarFeatures");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.RenameColumn(
                name: "FeatureId",
                table: "CarCarFeatures",
                newName: "CarFeatureId");

            migrationBuilder.RenameIndex(
                name: "IX_CarCarFeatures_FeatureId",
                table: "CarCarFeatures",
                newName: "IX_CarCarFeatures_CarFeatureId");

            migrationBuilder.CreateTable(
                name: "CarFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    NameAr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFeatures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarFeatures_NameAr",
                table: "CarFeatures",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarFeatures_NameEn",
                table: "CarFeatures",
                column: "NameEn",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarCarFeatures_CarFeatures_CarFeatureId",
                table: "CarCarFeatures",
                column: "CarFeatureId",
                principalTable: "CarFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
