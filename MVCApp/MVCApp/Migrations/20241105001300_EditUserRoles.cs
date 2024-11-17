using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCApp.Migrations
{
    /// <inheritdoc />
    public partial class EditUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "3a0483e7-c983-4a44-84c1-232cdf3bac36");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "5fb7f523-d4cf-4062-a94f-5a633e5116f3");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a11ba379-5ac9-4905-aa39-ea9e635e63b9");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "aecd6866-9b29-4b72-a573-15fff6a9607d");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4f7517b1-3e8d-40a1-a3c4-8c62ddb85ed7", null, "Admin", "ADMIN" },
                    { "91c634ae-b4e9-45ce-b1a1-c7fc6724d6d8", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "3a0483e7-c983-4a44-84c1-232cdf3bac36", null, "Director", "DIRECTOR" },
                    { "5fb7f523-d4cf-4062-a94f-5a633e5116f3", null, "Admin", "ADMIN" },
                    { "a11ba379-5ac9-4905-aa39-ea9e635e63b9", null, "Chef", "CHEF" },
                    { "aecd6866-9b29-4b72-a573-15fff6a9607d", null, "Waiter", "WAITER" }
                });
        }
    }
}
