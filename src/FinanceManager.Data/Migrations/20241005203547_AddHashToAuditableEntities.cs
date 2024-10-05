using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinanceManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHashToAuditableEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2de42fe8-1863-4cbe-8a40-756aa9c4678a"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b964a656-eca7-467e-a53d-927e174f3762"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ff5cf129-be98-48ae-8185-d2cd1f4c4f26"));

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Transactions",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Imports",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Categories",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Accounts",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Hash",
                value: "E5A1452E53371C3EBEB354A0A04CB5C5EC3C305DBC0C39821E76E0C659A53E38");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7cc1c322-3772-48bc-8003-352e1f7f4087"), null, "User", "USER" },
                    { new Guid("ba1304d6-ee73-4911-ae52-92de651c1f86"), null, "Admin", "ADMIN" },
                    { new Guid("ffd19195-7e92-4ed8-a181-b74f69521494"), null, "SystemAdmin", "SYSTEMADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7cc1c322-3772-48bc-8003-352e1f7f4087"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ba1304d6-ee73-4911-ae52-92de651c1f86"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ffd19195-7e92-4ed8-a181-b74f69521494"));

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Imports");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2de42fe8-1863-4cbe-8a40-756aa9c4678a"), null, "Admin", "ADMIN" },
                    { new Guid("b964a656-eca7-467e-a53d-927e174f3762"), null, "SystemAdmin", "SYSTEMADMIN" },
                    { new Guid("ff5cf129-be98-48ae-8185-d2cd1f4c4f26"), null, "User", "USER" }
                });
        }
    }
}
