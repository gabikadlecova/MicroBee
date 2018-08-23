using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroBee.Web.Migrations
{
    public partial class ItemUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerId",
                table: "MicroItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MicroItems_AspNetUsers_WorkerId",
                table: "MicroItems");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "MicroItems",
                newName: "WorkerName");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "MicroItems",
                newName: "OwnerName");

	        migrationBuilder.CreateIndex(
		        name: "IX_MicroItems_OwnerName",
		        table: "MicroItems",
		        column: "OwnerName");

	        migrationBuilder.CreateIndex(
		        name: "IX_MicroItems_WorkerName",
		        table: "MicroItems",
		        column: "WorkerName");

            migrationBuilder.AddForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerName",
                table: "MicroItems",
                column: "OwnerName",
                principalTable: "AspNetUsers",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MicroItems_AspNetUsers_WorkerName",
                table: "MicroItems",
                column: "WorkerName",
                principalTable: "AspNetUsers",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerName",
                table: "MicroItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MicroItems_AspNetUsers_WorkerName",
                table: "MicroItems");

            migrationBuilder.RenameColumn(
                name: "WorkerName",
                table: "MicroItems",
                newName: "WorkerName");

            migrationBuilder.RenameColumn(
                name: "OwnerName",
                table: "MicroItems",
                newName: "OwnerName");

			migrationBuilder.DropIndex(
		        name: "IX_MicroItems_OwnerName",
		        table: "MicroItems");

	        migrationBuilder.DropIndex(
		        name: "IX_MicroItems_WorkerName",
		        table: "MicroItems");

			migrationBuilder.AddForeignKey(
                name: "FK_MicroItems_AspNetUsers_OwnerId",
                table: "MicroItems",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MicroItems_AspNetUsers_WorkerId",
                table: "MicroItems",
                column: "WorkerId",
                principalTable: "AspNetUsers",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
