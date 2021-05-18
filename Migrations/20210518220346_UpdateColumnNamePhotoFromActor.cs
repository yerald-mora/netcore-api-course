using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCoreMoviesAPI.Migrations
{
    public partial class UpdateColumnNamePhotoFromActor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Foto",
                table: "Actors",
                newName: "Photo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "Actors",
                newName: "Foto");
        }
    }
}
