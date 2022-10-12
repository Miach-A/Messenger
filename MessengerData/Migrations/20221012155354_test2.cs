using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerData.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_Users_ContactGuid",
                table: "UserContacts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_Users_ContactGuid",
                table: "UserContacts",
                column: "ContactGuid",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_Users_ContactGuid",
                table: "UserContacts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_Users_ContactGuid",
                table: "UserContacts",
                column: "ContactGuid",
                principalTable: "Users",
                principalColumn: "Guid");
        }
    }
}
