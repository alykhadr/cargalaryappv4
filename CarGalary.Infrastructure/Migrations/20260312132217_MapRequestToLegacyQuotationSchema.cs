using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MapRequestToLegacyQuotationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No-op migration.
            // Model snapshot was updated to map Request entities to legacy quotation tables.
            // Keep database objects unchanged to avoid duplicate/rename conflicts.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No-op migration.
        }
    }
}
