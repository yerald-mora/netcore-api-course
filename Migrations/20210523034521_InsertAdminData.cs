using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCoreMoviesAPI.Migrations
{
    public partial class InsertAdminData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "735217af-72b5-4a8b-92b7-efe779101f16", "3682c3d0-45e3-4dc1-baad-07a6cf8be0bc", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4d686c30-d6af-4d54-b69e-8bc516219f86", 0, "6b34e49b-09f6-4fd0-ba9d-5baed11da31d", "ymora@hotmail.com", false, false, null, "ymora@hotmail.com", "ymora@hotmail.com", "AQAAAAEAACcQAAAAEDA9E+aQmzEIaDe5z61OJgwtv+TFEkFV80XXBEmyLpj0nXredNpypOz7ceECkkq2jA==", null, false, "0e6e9c88-97c3-4619-a210-6ab0fd054745", false, "ymora@hotmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "4d686c30-d6af-4d54-b69e-8bc516219f86" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "735217af-72b5-4a8b-92b7-efe779101f16");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d686c30-d6af-4d54-b69e-8bc516219f86");
        }
    }
}
