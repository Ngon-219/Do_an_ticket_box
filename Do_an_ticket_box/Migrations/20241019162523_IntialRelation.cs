using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Do_an_ticket_box.Migrations
{
    public partial class IntialRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ticket_Event_ID",
                table: "Ticket",
                column: "Event_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Report_Event_ID",
                table: "Report",
                column: "Event_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Report_User_ID",
                table: "Report",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Booking_ID",
                table: "Payment",
                column: "Booking_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_User_ID",
                table: "Booking",
                column: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_User_User_ID",
                table: "Booking",
                column: "User_ID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Booking_Booking_ID",
                table: "Payment",
                column: "Booking_ID",
                principalTable: "Booking",
                principalColumn: "Booking_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Event_Event_ID",
                table: "Report",
                column: "Event_ID",
                principalTable: "Event",
                principalColumn: "Event_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_User_ID",
                table: "Report",
                column: "User_ID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Event_Event_ID",
                table: "Ticket",
                column: "Event_ID",
                principalTable: "Event",
                principalColumn: "Event_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_User_User_ID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Booking_Booking_ID",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Event_Event_ID",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_User_ID",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Event_Event_ID",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_Event_ID",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Report_Event_ID",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_User_ID",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Payment_Booking_ID",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Booking_User_ID",
                table: "Booking");
        }
    }
}
