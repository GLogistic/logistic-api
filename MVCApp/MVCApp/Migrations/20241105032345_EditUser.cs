using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCApp.Migrations
{
    /// <inheritdoc />
    public partial class EditUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "4f7517b1-3e8d-40a1-a3c4-8c62ddb85ed7");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "91c634ae-b4e9-45ce-b1a1-c7fc6724d6d8");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "81dd72cf-a99f-4445-ac38-c293ad559f22", null, "User", "USER" },
                    { "86f8e540-963b-480b-9518-5a0b36ba0754", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "81dd72cf-a99f-4445-ac38-c293ad559f22");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "86f8e540-963b-480b-9518-5a0b36ba0754");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4f7517b1-3e8d-40a1-a3c4-8c62ddb85ed7", null, "Admin", "ADMIN" },
                    { "91c634ae-b4e9-45ce-b1a1-c7fc6724d6d8", null, "User", "USER" }
                });
        }
    }
}
