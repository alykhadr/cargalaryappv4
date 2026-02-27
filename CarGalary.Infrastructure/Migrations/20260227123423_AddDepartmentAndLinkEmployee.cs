using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentAndLinkEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                @"INSERT INTO Departments (NameAr, NameEn, CreatedBy, CreatedAt, IsAvailable)
                  SELECT DISTINCT LTRIM(RTRIM([Department])) AS NameAr,
                                  LTRIM(RTRIM([Department])) AS NameEn,
                                  'system',
                                  GETUTCDATE(),
                                  1
                  FROM Employees
                  WHERE [Department] IS NOT NULL AND LTRIM(RTRIM([Department])) <> '';");

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM Departments WHERE NameEn = 'General')
                  INSERT INTO Departments (NameAr, NameEn, CreatedBy, CreatedAt, IsAvailable)
                  VALUES (N'عام', 'General', 'system', GETUTCDATE(), 1);");

            migrationBuilder.Sql(
                @"UPDATE e
                  SET e.DepartmentId = d.Id
                  FROM Employees e
                  INNER JOIN Departments d ON LTRIM(RTRIM(e.[Department])) = d.NameEn;");

            migrationBuilder.Sql(
                @"UPDATE Employees
                  SET DepartmentId = (SELECT TOP 1 Id FROM Departments WHERE NameEn = 'General')
                  WHERE DepartmentId IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_NameAr",
                table: "Departments",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_NameEn",
                table: "Departments",
                column: "NameEn",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                @"UPDATE e
                  SET e.Department = ISNULL(d.NameEn, '')
                  FROM Employees e
                  LEFT JOIN Departments d ON e.DepartmentId = d.Id;");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
