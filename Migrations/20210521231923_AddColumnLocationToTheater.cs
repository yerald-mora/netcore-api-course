using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace NETCoreMoviesAPI.Migrations
{
    public partial class AddColumnLocationToTheater : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Theaters",
                type: "geography",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Theaters");
        }
    }
}
