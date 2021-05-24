using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCoreMoviesAPI.Migrations
{
    public partial class FixColumnTypeReviewsTableColumnCommentary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Commentary",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "735217af-72b5-4a8b-92b7-efe779101f16",
                column: "ConcurrencyStamp",
                value: "7fd1b9d7-176e-4787-a13d-943a855f1e4a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d686c30-d6af-4d54-b69e-8bc516219f86",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b461143-9bd6-430e-b142-deb1d49f2922", "AQAAAAEAACcQAAAAEA667GlWNqFQ2aRXnkOx8krl19SopQtlaX7yCnA8WpvdatWdW4XoXhLm4aqtBkvMSw==", "0356a42b-813e-4b29-9eb0-24b398993a4f" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Commentary",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
        }
    }
}
