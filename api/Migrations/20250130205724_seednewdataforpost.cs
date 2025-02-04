using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class seednewdataforpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd47c296-0839-4fe8-94cb-3e660a64e306");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ecdcfbfc-5f63-46e5-997e-1efbd9153671");

            migrationBuilder.AddColumn<string>(
                name: "HobbyId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "284e5b0f-586c-493e-987a-20d195f06617", null, "User", "USER" },
                    { "48713478-9bdb-4df2-b81f-7d5d824e723a", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "b67eb090-69b1-4255-9593-e80db3eb5f12", new DateTime(2025, 1, 30, 21, 57, 22, 984, DateTimeKind.Local).AddTicks(9487), "c29e0a62-b8e6-4ee1-8807-b0493b5d8ad4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "3bc45bf5-3e88-4550-9d3a-46168e89df3d", new DateTime(2025, 1, 30, 21, 57, 22, 985, DateTimeKind.Local).AddTicks(6071), "e26cc10a-cdb0-4397-a260-00cd3d68fd3a" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: "event1",
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 1, 21, 57, 22, 985, DateTimeKind.Local).AddTicks(7362), new DateTime(2025, 1, 31, 21, 57, 22, 985, DateTimeKind.Local).AddTicks(7212) });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_HobbyId",
                table: "Posts",
                column: "HobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Hobbies_HobbyId",
                table: "Posts",
                column: "HobbyId",
                principalTable: "Hobbies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Hobbies_HobbyId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_HobbyId",
                table: "Posts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "284e5b0f-586c-493e-987a-20d195f06617");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48713478-9bdb-4df2-b81f-7d5d824e723a");

            migrationBuilder.DropColumn(
                name: "HobbyId",
                table: "Posts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bd47c296-0839-4fe8-94cb-3e660a64e306", null, "Admin", "ADMIN" },
                    { "ecdcfbfc-5f63-46e5-997e-1efbd9153671", null, "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "03b8ca07-df95-4596-bc97-3baa6b439efc", new DateTime(2025, 1, 30, 0, 45, 34, 659, DateTimeKind.Local).AddTicks(749), "0f7ea544-0041-4537-ab52-dd4210d052c2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "195a80d7-4d92-46b3-a221-904bc40c35c3", new DateTime(2025, 1, 30, 0, 45, 34, 659, DateTimeKind.Local).AddTicks(9310), "153b0391-4bcc-49d6-ba05-fc8c383b549b" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: "event1",
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 1, 0, 45, 34, 660, DateTimeKind.Local).AddTicks(908), new DateTime(2025, 1, 31, 0, 45, 34, 660, DateTimeKind.Local).AddTicks(737) });
        }
    }
}
