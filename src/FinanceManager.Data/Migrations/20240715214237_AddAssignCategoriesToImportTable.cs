using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinanceManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignCategoriesToImportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("35541920-f7e1-448d-9e08-4220c03eaebf"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4ba94ced-1d0d-49cd-8534-604b3373465f"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f76f491d-b6b5-42df-81ed-2f3712bdafac"));

            migrationBuilder.AddColumn<ulong>(
                name: "AssignCategories",
                table: "Imports",
                type: "bit",
                nullable: false,
                defaultValue: 0ul);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "AssignCategories",
                table: "Imports");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("35541920-f7e1-448d-9e08-4220c03eaebf"), null, "Admin", "ADMIN" },
                    { new Guid("4ba94ced-1d0d-49cd-8534-604b3373465f"), null, "SystemAdmin", "SYSTEMADMIN" },
                    { new Guid("f76f491d-b6b5-42df-81ed-2f3712bdafac"), null, "User", "USER" }
                });
        }
    }
}
