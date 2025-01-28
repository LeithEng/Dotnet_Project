using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class fixHobbyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hobbies_AspNetUsers_UserId",
                table: "Hobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_Hobbies_Hobbies_ParentHobbyId",
                table: "Hobbies");

            migrationBuilder.DropIndex(
                name: "IX_Hobbies_UserId",
                table: "Hobbies");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Hobbies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hobbies");

            migrationBuilder.CreateTable(
                name: "FavoriteHobbies",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HobbyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteHobbies", x => new { x.UserId, x.HobbyId });
                    table.ForeignKey(
                        name: "FK_FavoriteHobbies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteHobbies_Hobbies_HobbyId",
                        column: x => x.HobbyId,
                        principalTable: "Hobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteHobbies_HobbyId",
                table: "FavoriteHobbies",
                column: "HobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hobbies_Hobbies_ParentHobbyId",
                table: "Hobbies",
                column: "ParentHobbyId",
                principalTable: "Hobbies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hobbies_Hobbies_ParentHobbyId",
                table: "Hobbies");

            migrationBuilder.DropTable(
                name: "FavoriteHobbies");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Hobbies",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Hobbies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hobbies_UserId",
                table: "Hobbies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hobbies_AspNetUsers_UserId",
                table: "Hobbies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hobbies_Hobbies_ParentHobbyId",
                table: "Hobbies",
                column: "ParentHobbyId",
                principalTable: "Hobbies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
