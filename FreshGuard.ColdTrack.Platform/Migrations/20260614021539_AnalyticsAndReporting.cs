using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace FreshGuard.ColdTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AnalyticsAndReporting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ReportCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    period_start = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    period_end = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    TotalShipments = table.Column<int>(type: "int", nullable: false),
                    CompletedShipments = table.Column<int>(type: "int", nullable: false),
                    TotalAlerts = table.Column<int>(type: "int", nullable: false),
                    AverageTemperature = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    AverageHumidity = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    GeneratedByUserId = table.Column<int>(type: "int", nullable: false),
                    GeneratedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ReportCode",
                table: "Report",
                column: "ReportCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Report");
        }
    }
}
