using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Do_an_ticket_box.Migrations
{
    public partial class Intial_creat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Booking_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    Event_ID = table.Column<int>(type: "int", nullable: false),
                    Ticket_ID = table.Column<int>(type: "int", nullable: false),
                    Booking_time = table.Column<byte[]>(type: "timestamp", nullable: false),
                    Total_amout = table.Column<decimal>(type: "decimal", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Booking_ID);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Event_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event_name = table.Column<string>(type: "varchar(150)", nullable: false),
                    Event_date = table.Column<DateTime>(type: "Date", nullable: false),
                    Event_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Location = table.Column<string>(type: "varchar(255)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Total_tickets = table.Column<int>(type: "int", nullable: false),
                    Avaiable_tickets = table.Column<int>(type: "int", nullable: false),
                    Created_at = table.Column<byte[]>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Event_ID);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Payment_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Booking_ID = table.Column<int>(type: "int", nullable: false),
                    Payment_time = table.Column<byte[]>(type: "timestamp", nullable: false),
                    Amount_paid = table.Column<decimal>(type: "decimal", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Payment_ID);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Report_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    Event_ID = table.Column<int>(type: "int", nullable: false),
                    Report = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<byte[]>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Report_ID);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Ticket_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event_ID = table.Column<int>(type: "int", nullable: false),
                    Ticket_type = table.Column<string>(type: "varchar(50)", nullable: false),
                    Price = table.Column<decimal>(type: "Decimal(10,2)", nullable: false),
                    Seat_number = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Seat_remain = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Ticket_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Ticket");
        }
    }
}
