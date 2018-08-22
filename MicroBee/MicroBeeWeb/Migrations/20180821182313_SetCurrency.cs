using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroBee.Web.Migrations
{
    public partial class SetCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "MicroItems",
                type: "Money",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "MicroItems",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money");
        }
    }
}
