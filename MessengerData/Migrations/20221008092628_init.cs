using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerData.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => new { x.Date, x.Guid });
                    table.ForeignKey(
                        name: "FK_Message_Chat_ChatGuid",
                        column: x => x.ChatGuid,
                        principalTable: "Chat",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_Message_User_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "User",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChats",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChats", x => new { x.ChatGuid, x.UserGuid });
                    table.ForeignKey(
                        name: "FK_UserChats_Chat_ChatGuid",
                        column: x => x.ChatGuid,
                        principalTable: "Chat",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChats_User_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "User",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "UserContacts",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContacts", x => new { x.UserGuid, x.ContactGuid });
                    table.ForeignKey(
                        name: "FK_UserContacts_User_ContactGuid",
                        column: x => x.ContactGuid,
                        principalTable: "User",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_UserContacts_User_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "User",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeletedMessage",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedMessage", x => new { x.Date, x.MessageGuid, x.ChatGuid, x.UserGuid });
                    table.ForeignKey(
                        name: "FK_DeletedMessage_Chat_ChatGuid",
                        column: x => x.ChatGuid,
                        principalTable: "Chat",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_DeletedMessage_Message_Date_MessageGuid",
                        columns: x => new { x.Date, x.MessageGuid },
                        principalTable: "Message",
                        principalColumns: new[] { "Date", "Guid" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeletedMessage_User_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "User",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "MessageComment",
                columns: table => new
                {
                    MessageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentedMessageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentedMessageGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageComment", x => new { x.MessageDate, x.MessageGuid, x.CommentedMessageDate, x.CommentedMessageGuid });
                    table.ForeignKey(
                        name: "FK_MessageComment_Message_CommentedMessageDate_CommentedMessageGuid",
                        columns: x => new { x.CommentedMessageDate, x.CommentedMessageGuid },
                        principalTable: "Message",
                        principalColumns: new[] { "Date", "Guid" });
                    table.ForeignKey(
                        name: "FK_MessageComment_Message_MessageDate_MessageGuid",
                        columns: x => new { x.MessageDate, x.MessageGuid },
                        principalTable: "Message",
                        principalColumns: new[] { "Date", "Guid" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeletedMessage_ChatGuid",
                table: "DeletedMessage",
                column: "ChatGuid");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedMessage_UserGuid",
                table: "DeletedMessage",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatGuid",
                table: "Message",
                column: "ChatGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserGuid",
                table: "Message",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_MessageComment_CommentedMessageDate_CommentedMessageGuid",
                table: "MessageComment",
                columns: new[] { "CommentedMessageDate", "CommentedMessageGuid" });

            migrationBuilder.CreateIndex(
                name: "IX_MessageComment_MessageDate_MessageGuid",
                table: "MessageComment",
                columns: new[] { "MessageDate", "MessageGuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChats_UserGuid",
                table: "UserChats",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_ContactGuid",
                table: "UserContacts",
                column: "ContactGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedMessage");

            migrationBuilder.DropTable(
                name: "MessageComment");

            migrationBuilder.DropTable(
                name: "UserChats");

            migrationBuilder.DropTable(
                name: "UserContacts");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
