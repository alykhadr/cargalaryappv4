using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VehicleOwnerType = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Quotations_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_LookupDetails_CityId",
                        column: x => x.CityId,
                        principalTable: "LookupDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_LookupDetails_PaymentMethod",
                        column: x => x.PaymentMethod,
                        principalTable: "LookupDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_LookupDetails_RegionId",
                        column: x => x.RegionId,
                        principalTable: "LookupDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_LookupDetails_VehicleOwnerType",
                        column: x => x.VehicleOwnerType,
                        principalTable: "LookupDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CarId",
                table: "Quotations",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CityId",
                table: "Quotations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_PaymentMethod",
                table: "Quotations",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_RegionId",
                table: "Quotations",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_UserId",
                table: "Quotations",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_VehicleOwnerType",
                table: "Quotations",
                column: "VehicleOwnerType");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'PAYMENT_METHOD' AND DetailCode = '1')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('PAYMENT_METHOD', '1', N'نقداً', 'Cash', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'PAYMENT_METHOD' AND DetailCode = '2')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('PAYMENT_METHOD', '2', N'تمويل', 'Finance', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'VEHICLE_OWNER_TYPE' AND DetailCode = '1')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('VEHICLE_OWNER_TYPE', '1', N'فرد', 'Individual', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'VEHICLE_OWNER_TYPE' AND DetailCode = '2')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('VEHICLE_OWNER_TYPE', '2', N'شركة', 'Company', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'VEHICLE_OWNER_TYPE' AND DetailCode = '3')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('VEHICLE_OWNER_TYPE', '3', N'خدمة', 'Service', NULL, 'system', GETUTCDATE(), 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DELETE FROM LookupDetails
                  WHERE MasterCode IN ('PAYMENT_METHOD', 'VEHICLE_OWNER_TYPE')
                    AND DetailCode IN ('1','2','3');");

            migrationBuilder.DropTable(
                name: "Quotations");
        }
    }
}
