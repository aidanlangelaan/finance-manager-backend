using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinanceManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1b32bce9-dc26-406e-bdb0-a0b03281c296"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9b2f4a06-98dd-49ec-8188-cf01f2c5ed95"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f2ae361a-cd28-4a5d-8686-9b024bcb66be"));

            migrationBuilder.AddColumn<int>(
                name: "ImportId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Imports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OriginalFileName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TemporaryFileName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankType = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    RowVersion = table.Column<DateTime>(type: "datetime", rowVersion: true, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CreatedOnAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UpdatedOnAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imports", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1896c355-6550-4f28-ae63-176073b9454d"), null, "SystemAdmin", "SYSTEMADMIN" },
                    { new Guid("c4007b7e-a94e-44b5-b1ef-c05f39030899"), null, "Admin", "ADMIN" },
                    { new Guid("f0110143-a9d8-4602-83ac-ffe54fa32188"), null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ImportId",
                table: "Transactions",
                column: "ImportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Imports_ImportId",
                table: "Transactions",
                column: "ImportId",
                principalTable: "Imports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Imports_ImportId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Imports");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ImportId",
                table: "Transactions");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1896c355-6550-4f28-ae63-176073b9454d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c4007b7e-a94e-44b5-b1ef-c05f39030899"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f0110143-a9d8-4602-83ac-ffe54fa32188"));

            migrationBuilder.DropColumn(
                name: "ImportId",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1b32bce9-dc26-406e-bdb0-a0b03281c296"), null, "Admin", "ADMIN" },
                    { new Guid("9b2f4a06-98dd-49ec-8188-cf01f2c5ed95"), null, "SystemAdmin", "SYSTEMADMIN" },
                    { new Guid("f2ae361a-cd28-4a5d-8686-9b024bcb66be"), null, "User", "USER" }
                });
        }
    }
}
