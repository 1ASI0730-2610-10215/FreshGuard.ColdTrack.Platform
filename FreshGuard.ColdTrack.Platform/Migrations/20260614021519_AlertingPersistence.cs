using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace FreshGuard.ColdTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AlertingPersistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AlertCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Severity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    Limit = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    TriggeredAt = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    AcknowledgedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    ResolvedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    ResolvedByUserId = table.Column<int>(type: "int", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_AlertCode",
                table: "Alert",
                column: "AlertCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alert_ShipmentId_Status",
                table: "Alert",
                columns: new[] { "ShipmentId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alert");
        }
    }
}
