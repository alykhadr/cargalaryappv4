using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeUserLockoutEndRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE AspNetUsers
SET LockoutEnd = '2077-11-16T23:59:59+00:00'
WHERE LockoutEnd IS NOT NULL
  AND LockoutEnd > '2077-11-16T23:59:59+00:00';
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
