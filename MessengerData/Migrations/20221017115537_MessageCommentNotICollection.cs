using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerData.Migrations
{
    public partial class MessageCommentNotICollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageComment",
                table: "MessageComment");

            migrationBuilder.DropIndex(
                name: "IX_MessageComment_MessageDate_MessageGuid",
                table: "MessageComment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageComment",
                table: "MessageComment",
                columns: new[] { "MessageDate", "MessageGuid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageComment",
                table: "MessageComment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageComment",
                table: "MessageComment",
                columns: new[] { "MessageDate", "MessageGuid", "CommentedMessageDate", "CommentedMessageGuid" });

            migrationBuilder.CreateIndex(
                name: "IX_MessageComment_MessageDate_MessageGuid",
                table: "MessageComment",
                columns: new[] { "MessageDate", "MessageGuid" },
                unique: true);
        }
    }
}
