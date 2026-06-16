using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshGuard.ColdTrack.Platform.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20260616050000_RepairCoreSchema")]
    public partial class RepairCoreSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `UserAccount` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `FullName` varchar(120) NOT NULL,
                    `Email` varchar(254) NOT NULL,
                    `PasswordHash` varchar(255) NOT NULL,
                    `Role` varchar(30) NOT NULL,
                    `IsActive` tinyint(1) NOT NULL,
                    `CreatedAt` datetime NULL,
                    `UpdatedAt` datetime NULL,
                    PRIMARY KEY (`Id`)
                ) CHARACTER SET utf8mb4;
                """);

            CreateIndexIfMissing(migrationBuilder, "UserAccount", "IX_UserAccount_Email",
                "CREATE UNIQUE INDEX `IX_UserAccount_Email` ON `UserAccount` (`Email`)");

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `Shipment` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `ShipmentCode` varchar(20) NOT NULL,
                    `Destination` varchar(150) NOT NULL,
                    `DriverId` int NOT NULL,
                    `CargoDescription` varchar(500) NOT NULL,
                    `DepartureDate` datetime NOT NULL,
                    `EstimatedArrival` datetime NOT NULL,
                    `ActualArrival` datetime NULL,
                    `Status` varchar(30) NOT NULL,
                    `CreatedAt` datetime NULL,
                    `UpdatedAt` datetime NULL,
                    PRIMARY KEY (`Id`)
                ) CHARACTER SET utf8mb4;
                """);

            CreateIndexIfMissing(migrationBuilder, "Shipment", "IX_Shipment_ShipmentCode",
                "CREATE UNIQUE INDEX `IX_Shipment_ShipmentCode` ON `Shipment` (`ShipmentCode`)");

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `ShipmentStatusHistory` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `ShipmentId` int NOT NULL,
                    `PreviousStatus` varchar(30) NOT NULL,
                    `NewStatus` varchar(30) NOT NULL,
                    `ChangedByUserId` int NOT NULL,
                    `Remarks` varchar(300) NULL,
                    `ChangedAt` datetime NOT NULL,
                    PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_ShipmentStatusHistory_Shipment_ShipmentId`
                        FOREIGN KEY (`ShipmentId`) REFERENCES `Shipment` (`Id`) ON DELETE CASCADE
                ) CHARACTER SET utf8mb4;
                """);

            CreateIndexIfMissing(migrationBuilder, "ShipmentStatusHistory", "IX_ShipmentStatusHistory_ShipmentId",
                "CREATE INDEX `IX_ShipmentStatusHistory_ShipmentId` ON `ShipmentStatusHistory` (`ShipmentId`)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS `ShipmentStatusHistory`;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS `Shipment`;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS `UserAccount`;");
        }

        private static void CreateIndexIfMissing(MigrationBuilder migrationBuilder, string tableName, string indexName,
            string createIndexSql)
        {
            migrationBuilder.Sql($"""
                SET @index_exists := (
                    SELECT COUNT(1)
                    FROM INFORMATION_SCHEMA.STATISTICS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = '{tableName}'
                      AND INDEX_NAME = '{indexName}'
                );
                SET @create_index_sql := IF(@index_exists = 0, '{createIndexSql}', 'SELECT 1');
                PREPARE create_index_statement FROM @create_index_sql;
                EXECUTE create_index_statement;
                DEALLOCATE PREPARE create_index_statement;
                """);
        }
    }
}
