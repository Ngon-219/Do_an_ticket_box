using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Do_an_ticket_box.Migrations
{
    public partial class AddImgEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Event_image",
                table: "Event",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Event_image",
                table: "Event");
        }
    }
}
