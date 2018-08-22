using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroBee.Web.Migrations
{
    public partial class AddedPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerId",
                table: "MicroItems");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "MicroItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "MicroItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerId",
                table: "MicroItems",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerId",
                table: "MicroItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "MicroItems");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "MicroItems",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerId",
                table: "MicroItems",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
