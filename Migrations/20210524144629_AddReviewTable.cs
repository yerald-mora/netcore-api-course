using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCoreMoviesAPI.Migrations
{
    public partial class AddReviewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Commentary = table.Column<int>(type: "int", nullable: false),
                    Punctuation = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "735217af-72b5-4a8b-92b7-efe779101f16",
                column: "ConcurrencyStamp",
                value: "291467ac-2287-47b0-a988-6a10b59a5466");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d686c30-d6af-4d54-b69e-8bc516219f86",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e693bc0b-4fc7-41f0-ade6-5a9ca6943a3f", "AQAAAAEAACcQAAAAEKyD5TpR7A4DW2jE+kzzq3XszeTSTm11/Hu3W+VYI+C0kuPl0AbEGELHUetDXbvPMQ==", "376acb9f-9a0a-4398-9c12-948aab2cf86e" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "735217af-72b5-4a8b-92b7-efe779101f16",
                column: "ConcurrencyStamp",
                value: "3682c3d0-45e3-4dc1-baad-07a6cf8be0bc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d686c30-d6af-4d54-b69e-8bc516219f86",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6b34e49b-09f6-4fd0-ba9d-5baed11da31d", "AQAAAAEAACcQAAAAEDA9E+aQmzEIaDe5z61OJgwtv+TFEkFV80XXBEmyLpj0nXredNpypOz7ceECkkq2jA==", "0e6e9c88-97c3-4619-a210-6ab0fd054745" });
        }
    }
}
