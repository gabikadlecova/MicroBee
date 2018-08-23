using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroBee.Web.Migrations
{
    public partial class UserItemRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "MicroItems");

            migrationBuilder.AddColumn<string>(
                name: "ImageAddress",
                table: "MicroItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "MicroItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "MicroItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            
            migrationBuilder.AddForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerId",
                table: "MicroItems",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MicroItems_AspNetUsers_WorkerId",
                table: "MicroItems",
                column: "WorkerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerId",
                table: "MicroItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MicroItems_AspNetUsers_WorkerId",
                table: "MicroItems");

            migrationBuilder.DropTable(
                name: "Images");

           

            migrationBuilder.DropColumn(
                name: "ImageAddress",
                table: "MicroItems");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "MicroItems");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "MicroItems");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "MicroItems",
                nullable: true);
        }
    }
}
