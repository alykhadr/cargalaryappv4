using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestStatusAndHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentStatus",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentStatusDate",
                table: "Requests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REQUEST_STATUS' AND DetailCode = '1')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REQUEST_STATUS', '1', N'جديد', 'New', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REQUEST_STATUS' AND DetailCode = '2')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REQUEST_STATUS', '2', N'قيد المتابعة', 'In Progress', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REQUEST_STATUS' AND DetailCode = '3')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REQUEST_STATUS', '3', N'تم التواصل', 'Contacted', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REQUEST_STATUS' AND DetailCode = '4')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REQUEST_STATUS', '4', N'مغلق - ناجح', 'Closed Won', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM LookupDetails WHERE MasterCode = 'REQUEST_STATUS' AND DetailCode = '5')
                  INSERT INTO LookupDetails (MasterCode, DetailCode, NameAr, NameEn, MappedCode, CreatedBy, CreatedAt, IsAvailable)
                  VALUES ('REQUEST_STATUS', '5', N'مغلق - ملغي', 'Closed Lost', NULL, 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"UPDATE q
                  SET q.CurrentStatus = ls.Id,
                      q.CurrentStatusDate = ISNULL(q.CreatedAt, GETUTCDATE())
                  FROM Requests q
                  CROSS APPLY (
                    SELECT TOP 1 Id
                    FROM LookupDetails
                    WHERE MasterCode = 'REQUEST_STATUS' AND DetailCode = '1'
                    ORDER BY Id
                  ) ls
                  WHERE q.CurrentStatus IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentStatus",
                table: "Requests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CurrentStatusDate",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RequestHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestHistories_LookupDetails_Status",
                        column: x => x.Status,
                        principalTable: "LookupDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestHistories_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrentStatus",
                table: "Requests",
                column: "CurrentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_RequestId",
                table: "RequestHistories",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_Status",
                table: "RequestHistories",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_LookupDetails_CurrentStatus",
                table: "Requests",
                column: "CurrentStatus",
                principalTable: "LookupDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(
                @"INSERT INTO RequestHistories (RequestId, Status, StatusDate, Notes, CreatedAt, IsAvailable)
                  SELECT q.Id, q.CurrentStatus, q.CurrentStatusDate, 'Initial status', GETUTCDATE(), 1
                  FROM Requests q
                  WHERE NOT EXISTS (
                    SELECT 1
                    FROM RequestHistories h
                    WHERE h.RequestId = q.Id
                  );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_LookupDetails_CurrentStatus",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "RequestHistories");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CurrentStatus",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CurrentStatus",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CurrentStatusDate",
                table: "Requests");

            migrationBuilder.Sql(
                @"DELETE FROM LookupDetails
                  WHERE MasterCode = 'REQUEST_STATUS'
                    AND DetailCode IN ('1','2','3','4','5');");
        }
    }
}
