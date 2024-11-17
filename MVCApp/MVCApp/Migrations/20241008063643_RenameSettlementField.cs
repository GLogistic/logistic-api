using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameSettlementField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Customers_EndSettlementId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Customers_StartSettlementId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Settlements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settlements",
                table: "Settlements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Settlements_EndSettlementId",
                table: "Routes",
                column: "EndSettlementId",
                principalTable: "Settlements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Settlements_StartSettlementId",
                table: "Routes",
                column: "StartSettlementId",
                principalTable: "Settlements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Settlements_EndSettlementId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Settlements_StartSettlementId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Settlements",
                table: "Settlements");

            migrationBuilder.RenameTable(
                name: "Settlements",
                newName: "Customers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Customers_EndSettlementId",
                table: "Routes",
                column: "EndSettlementId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Customers_StartSettlementId",
                table: "Routes",
                column: "StartSettlementId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
