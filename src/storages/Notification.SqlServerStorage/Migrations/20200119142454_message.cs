using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Desyco.Notification.SqlServerStorage.Migrations
{
    public partial class message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dls");

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "dls",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    To = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    ReplyTo = table.Column<string>(nullable: true),
                    TemplateKey = table.Column<string>(maxLength: 255, nullable: true),
                    DeliveryDate = table.Column<DateTime>(nullable: true),
                    UrgencyLevel = table.Column<int>(nullable: false),
                    NotificationMethod = table.Column<int>(nullable: false),
                    TextFormat = table.Column<int>(nullable: false),
                    Group = table.Column<string>(maxLength: 36, nullable: true),
                    Subject = table.Column<string>(maxLength: 200, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DeliveryAttempts = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationAttachment",
                schema: "dls",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    MessageId = table.Column<string>(maxLength: 36, nullable: true),
                    FileName = table.Column<string>(maxLength: 255, nullable: true),
                    MediaType = table.Column<string>(maxLength: 30, nullable: true),
                    MediaSubType = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationAttachment_Notification_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "dls",
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationError",
                schema: "dls",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    MessageId = table.Column<string>(maxLength: 36, nullable: true),
                    DeliveryAttempts = table.Column<int>(nullable: false),
                    ErrorTime = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationError_Notification_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "dls",
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationAttachment_MessageId",
                schema: "dls",
                table: "NotificationAttachment",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationError_MessageId",
                schema: "dls",
                table: "NotificationError",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationAttachment",
                schema: "dls");

            migrationBuilder.DropTable(
                name: "NotificationError",
                schema: "dls");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "dls");
        }
    }
}
