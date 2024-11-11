using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Do_an_ticket_box.Migrations
{
    public partial class addEndDateEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_Booking_ID",
                table: "Payment");

            migrationBuilder.AddColumn<DateTime>(
                name: "Event_date_end",
                table: "Event",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Event_time_end",
                table: "Event",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Booking_ID",
                table: "Payment",
                column: "Booking_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_Booking_ID",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Event_date_end",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Event_time_end",
                table: "Event");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Booking_ID",
                table: "Payment",
                column: "Booking_ID",
                unique: true,
                filter: "[Booking_ID] IS NOT NULL");
        }
    }
}
