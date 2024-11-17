using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRestrictionToRouteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Route_DifferentSettlements",
                table: "Routes",
                sql: "[StartSettlementId] <> [EndSettlementId]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Route_DifferentSettlements",
                table: "Routes");
        }
    }
}
