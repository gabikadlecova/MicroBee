using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroBee.Web.Migrations
{
    public partial class ImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageAddress",
                table: "MicroItems");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "MicroItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "MicroItems");

            migrationBuilder.AddColumn<string>(
                name: "ImageAddress",
                table: "MicroItems",
                nullable: true);
        }
    }
}
