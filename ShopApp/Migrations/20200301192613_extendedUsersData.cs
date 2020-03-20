using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApp.Migrations
{
    public partial class extendedUsersData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfregistration",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "AccountDataId",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountDatas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    TimeOfRegistration = table.Column<DateTime>(nullable: false),
                    TimeOfRemovingAccount = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDatas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountDataId",
                table: "Users",
                column: "AccountDataId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AccountDatas_AccountDataId",
                table: "Users",
                column: "AccountDataId",
                principalTable: "AccountDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AccountDatas_AccountDataId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AccountDatas");

            migrationBuilder.DropIndex(
                name: "IX_Users_AccountDataId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccountDataId",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfregistration",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
