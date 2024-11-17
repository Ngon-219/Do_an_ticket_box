using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Do_an_ticket_box.Migrations
{
    public partial class Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmailVerificationTokenId",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmailVerificationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerificationTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_EmailVerificationTokenId",
                table: "User",
                column: "EmailVerificationTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_EmailVerificationTokens_EmailVerificationTokenId",
                table: "User",
                column: "EmailVerificationTokenId",
                principalTable: "EmailVerificationTokens",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_EmailVerificationTokens_EmailVerificationTokenId",
                table: "User");

            migrationBuilder.DropTable(
                name: "EmailVerificationTokens");

            migrationBuilder.DropIndex(
                name: "IX_User_EmailVerificationTokenId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailVerificationTokenId",
                table: "User");
        }
    }
}
