using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApp.Migrations
{
    public partial class ability_delete_and_cancel_anything : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ProductDeleted",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdWholeOrder",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CommentDeleted",
                table: "Comments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IdWholeOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CommentDeleted",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserDeleted",
                table: "AspNetUsers");
        }
    }
}
