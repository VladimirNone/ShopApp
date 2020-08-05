using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApp.Migrations
{
    public partial class addedDetailsAboutOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinalLocation",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForCancellation",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FinalLocation",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReasonForCancellation",
                table: "Orders");
        }
    }
}
