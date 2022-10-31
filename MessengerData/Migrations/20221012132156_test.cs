using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerData.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_Users_UserGuid",
                table: "UserContacts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_Users_UserGuid",
                table: "UserContacts",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_Users_UserGuid",
                table: "UserContacts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_Users_UserGuid",
                table: "UserContacts",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
