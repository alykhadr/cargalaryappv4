using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameManufactureCountryMasterCodeToCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE LookupDetails
                  SET MasterCode = 'COUNTRY'
                  WHERE MasterCode = 'MANUFACTURE_COUNTRY';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE LookupDetails
                  SET MasterCode = 'MANUFACTURE_COUNTRY'
                  WHERE MasterCode = 'COUNTRY';");
        }
    }
}
