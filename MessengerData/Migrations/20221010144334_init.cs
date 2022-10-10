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
                name: "Chats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Public = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ChatGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => new { x.Date, x.Guid });
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatGuid",
                        column: x => x.ChatGuid,
                        principalTable: "Chats",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
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
                        name: "FK_UserChats_Chats_ChatGuid",
                        column: x => x.ChatGuid,
                        principalTable: "Chats",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChats_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
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
                        name: "FK_UserContacts_Users_ContactGuid",
                        column: x => x.ContactGuid,
                        principalTable: "Users",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_UserContacts_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
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
                        name: "FK_DeletedMessage_Chats_ChatGuid",
                        column: x => x.ChatGuid,
                        principalTable: "Chats",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_DeletedMessage_Messages_Date_MessageGuid",
                        columns: x => new { x.Date, x.MessageGuid },
                        principalTable: "Messages",
                        principalColumns: new[] { "Date", "Guid" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeletedMessage_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
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
                        name: "FK_MessageComment_Messages_CommentedMessageDate_CommentedMessageGuid",
                        columns: x => new { x.CommentedMessageDate, x.CommentedMessageGuid },
                        principalTable: "Messages",
                        principalColumns: new[] { "Date", "Guid" });
                    table.ForeignKey(
                        name: "FK_MessageComment_Messages_MessageDate_MessageGuid",
                        columns: x => new { x.MessageDate, x.MessageGuid },
                        principalTable: "Messages",
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
                name: "IX_MessageComment_CommentedMessageDate_CommentedMessageGuid",
                table: "MessageComment",
                columns: new[] { "CommentedMessageDate", "CommentedMessageGuid" });

            migrationBuilder.CreateIndex(
                name: "IX_MessageComment_MessageDate_MessageGuid",
                table: "MessageComment",
                columns: new[] { "MessageDate", "MessageGuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatGuid",
                table: "Messages",
                column: "ChatGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserGuid",
                table: "Messages",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserChats_UserGuid",
                table: "UserChats",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_ContactGuid",
                table: "UserContacts",
                column: "ContactGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
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
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
