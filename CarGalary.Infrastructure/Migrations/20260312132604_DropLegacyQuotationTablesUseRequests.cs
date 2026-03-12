using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropLegacyQuotationTablesUseRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- 1) Ensure Requests table exists and migrate data from legacy Quotations if needed
IF OBJECT_ID(N'dbo.Quotations', N'U') IS NOT NULL
BEGIN
    IF OBJECT_ID(N'dbo.Requests', N'U') IS NULL
    BEGIN
        EXEC sp_rename N'dbo.Quotations', N'Requests', N'OBJECT';
    END
    ELSE
    BEGIN
        SET IDENTITY_INSERT dbo.Requests ON;

        INSERT INTO dbo.Requests
        (
            Id, UserId, VehicleOwnerType, PaymentMethod, RegionId, CityId,
            CurrentStatus, CurrentStatusDate, Name, Email, MobileNo, CarId,
            Notes, CreatedAt, UpdatedAt, UpdatedBy, IsAvailable
        )
        SELECT
            q.Id, q.UserId, q.VehicleOwnerType, q.PaymentMethod, q.RegionId, q.CityId,
            q.CurrentStatus, q.CurrentStatusDate, q.Name, q.Email, q.MobileNo, q.CarId,
            q.Notes, q.CreatedAt, q.UpdatedAt, q.UpdatedBy, q.IsAvailable
        FROM dbo.Quotations q
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Requests r WHERE r.Id = q.Id);

        SET IDENTITY_INSERT dbo.Requests OFF;
    END
END
");

            migrationBuilder.Sql(@"
-- 2) Ensure RequestHistories table exists and migrate data from legacy QuotationHistories
IF OBJECT_ID(N'dbo.QuotationHistories', N'U') IS NOT NULL
BEGIN
    IF OBJECT_ID(N'dbo.RequestHistories', N'U') IS NULL
    BEGIN
        EXEC sp_rename N'dbo.QuotationHistories', N'RequestHistories', N'OBJECT';
    END
    ELSE
    BEGIN
        SET IDENTITY_INSERT dbo.RequestHistories ON;
        DECLARE @mergeHistSql NVARCHAR(MAX);
        IF COL_LENGTH('dbo.QuotationHistories', 'RequestId') IS NOT NULL
        BEGIN
            SET @mergeHistSql = N'
                INSERT INTO dbo.RequestHistories
                (
                    Id, RequestId, Status, StatusDate, Notes,
                    CreatedAt, UpdatedAt, UpdatedBy, IsAvailable
                )
                SELECT
                    qh.Id, qh.RequestId, qh.Status, qh.StatusDate, qh.Notes,
                    qh.CreatedAt, qh.UpdatedAt, qh.UpdatedBy, qh.IsAvailable
                FROM dbo.QuotationHistories qh
                WHERE NOT EXISTS (SELECT 1 FROM dbo.RequestHistories rh WHERE rh.Id = qh.Id);';
        END
        ELSE
        BEGIN
            SET @mergeHistSql = N'
                INSERT INTO dbo.RequestHistories
                (
                    Id, RequestId, Status, StatusDate, Notes,
                    CreatedAt, UpdatedAt, UpdatedBy, IsAvailable
                )
                SELECT
                    qh.Id, qh.QuotationId, qh.Status, qh.StatusDate, qh.Notes,
                    qh.CreatedAt, qh.UpdatedAt, qh.UpdatedBy, qh.IsAvailable
                FROM dbo.QuotationHistories qh
                WHERE NOT EXISTS (SELECT 1 FROM dbo.RequestHistories rh WHERE rh.Id = qh.Id);';
        END

        EXEC sp_executesql @mergeHistSql;

        SET IDENTITY_INSERT dbo.RequestHistories OFF;
    END
END
");

            migrationBuilder.Sql(@"
-- 3) If RequestHistories still uses QuotationId column name, rename it to RequestId
IF OBJECT_ID(N'dbo.RequestHistories', N'U') IS NOT NULL
   AND COL_LENGTH('dbo.RequestHistories', 'QuotationId') IS NOT NULL
   AND COL_LENGTH('dbo.RequestHistories', 'RequestId') IS NULL
BEGIN
    EXEC sp_rename N'dbo.RequestHistories.QuotationId', N'RequestId', N'COLUMN';
END
");

            migrationBuilder.Sql(@"
-- 4) Drop legacy QuotationHistories table if still present (drop all FKs first)
IF OBJECT_ID(N'dbo.QuotationHistories', N'U') IS NOT NULL
BEGIN
    DECLARE @sqlQH NVARCHAR(MAX) = N'';
    SELECT @sqlQH = @sqlQH + N'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) + N'].[' + OBJECT_NAME(parent_object_id) + N'] DROP CONSTRAINT [' + name + N'];'
    FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'dbo.QuotationHistories')
       OR referenced_object_id = OBJECT_ID(N'dbo.QuotationHistories');

    IF LEN(@sqlQH) > 0 EXEC sp_executesql @sqlQH;

    DROP TABLE dbo.QuotationHistories;
END
");

            migrationBuilder.Sql(@"
-- 5) Drop legacy Quotations table if still present (drop all FKs first)
IF OBJECT_ID(N'dbo.Quotations', N'U') IS NOT NULL
BEGIN
    DECLARE @sqlQ NVARCHAR(MAX) = N'';
    SELECT @sqlQ = @sqlQ + N'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) + N'].[' + OBJECT_NAME(parent_object_id) + N'] DROP CONSTRAINT [' + name + N'];'
    FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'dbo.Quotations')
       OR referenced_object_id = OBJECT_ID(N'dbo.Quotations');

    IF LEN(@sqlQ) > 0 EXEC sp_executesql @sqlQ;

    DROP TABLE dbo.Quotations;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Irreversible by design: this migration consolidates data and removes legacy tables.
        }
    }
}
