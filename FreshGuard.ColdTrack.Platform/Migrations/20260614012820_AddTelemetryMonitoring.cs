using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace FreshGuard.ColdTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddTelemetryMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sensor_code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    model_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    shipment_id = table.Column<int>(type: "int", nullable: true),
                    assigned_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    last_reading_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    temperature = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    humidity = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensors", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "telemetry_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sensor_id = table.Column<int>(type: "int", nullable: false),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    temperature = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    humidity = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    recorded_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_telemetry_logs", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_sensors_sensor_code",
                table: "sensors",
                column: "sensor_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_telemetry_logs_shipment_id_recorded_at",
                table: "telemetry_logs",
                columns: new[] { "shipment_id", "recorded_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "telemetry_logs");
        }
    }
}
