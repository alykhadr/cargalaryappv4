using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRateEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CarId = table.Column<int>(type: "int", nullable: true),
                    ReviewerNameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReviewerNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CommentAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CommentEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RateValue = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    IsProductReview = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRates", x => x.Id);
                    table.CheckConstraint("CK_UserRates_RateValue", "[RateValue] >= 1 AND [RateValue] <= 5");
                    table.ForeignKey(
                        name: "FK_UserRates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserRates_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRates_CarId",
                table: "UserRates",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRates_IsAvailable_IsProductReview_CreatedAt",
                table: "UserRates",
                columns: new[] { "IsAvailable", "IsProductReview", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRates_UserId",
                table: "UserRates",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRates");
        }
    }
}
