using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCApp.Migrations
{
    /// <inheritdoc />
    public partial class EditUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole<Guid>",
                keyColumn: "Id",
                keyValue: new Guid("161fc3c7-3924-4df8-9a3b-ad8f55d461f4"));

            migrationBuilder.DeleteData(
                table: "IdentityRole<Guid>",
                keyColumn: "Id",
                keyValue: new Guid("ea39f76d-9e03-4599-88dc-2866edbaab14"));

            migrationBuilder.InsertData(
                table: "IdentityRole<Guid>",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3781aefc-60a2-4edc-8316-22105d1c6341"), null, "Admin", "ADMIN" },
                    { new Guid("623a6894-b314-4c86-bceb-aa8fc8c37496"), null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole<Guid>",
                keyColumn: "Id",
                keyValue: new Guid("3781aefc-60a2-4edc-8316-22105d1c6341"));

            migrationBuilder.DeleteData(
                table: "IdentityRole<Guid>",
                keyColumn: "Id",
                keyValue: new Guid("623a6894-b314-4c86-bceb-aa8fc8c37496"));

            migrationBuilder.InsertData(
                table: "IdentityRole<Guid>",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("161fc3c7-3924-4df8-9a3b-ad8f55d461f4"), null, "User", "USER" },
                    { new Guid("ea39f76d-9e03-4599-88dc-2866edbaab14"), null, "Admin", "ADMIN" }
                });
        }
    }
}
