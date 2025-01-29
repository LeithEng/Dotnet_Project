using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class TokenBlackListtable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenBlacklists",
                table: "TokenBlacklists");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "416b6bc7-18b1-4a66-b584-e32c53e145c5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6416e3d2-3d88-48ac-a580-c4cd15eeb064");

            migrationBuilder.RenameTable(
                name: "TokenBlacklists",
                newName: "TokenBlacklist");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenBlacklist",
                table: "TokenBlacklist",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2710fa00-2818-4006-ac2c-0aff9693c2d1", null, "Admin", "ADMIN" },
                    { "777a6f6b-cbc4-4eae-942c-51ebc66d3dc4", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenBlacklist",
                table: "TokenBlacklist");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2710fa00-2818-4006-ac2c-0aff9693c2d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "777a6f6b-cbc4-4eae-942c-51ebc66d3dc4");

            migrationBuilder.RenameTable(
                name: "TokenBlacklist",
                newName: "TokenBlacklists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenBlacklists",
                table: "TokenBlacklists",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "416b6bc7-18b1-4a66-b584-e32c53e145c5", null, "Admin", "ADMIN" },
                    { "6416e3d2-3d88-48ac-a580-c4cd15eeb064", null, "User", "USER" }
                });
        }
    }
}
