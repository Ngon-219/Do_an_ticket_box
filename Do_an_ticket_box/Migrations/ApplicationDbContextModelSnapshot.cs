﻿// <auto-generated />
using System;
using Do_an_ticket_box.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Do_an_ticket_box.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Do_an_ticket_box.Models.Booking", b =>
                {
                    b.Property<int>("Booking_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Booking_ID"), 1L, 1);

                    b.Property<int?>("Event_ID")
                        .HasColumnType("int");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quanlity")
                        .HasColumnType("int")
                        .HasColumnName("Quanlity");

                    b.Property<int?>("Ticket_ID")
                        .HasColumnType("int");

                    b.Property<int?>("User_ID")
                        .HasColumnType("int");

                    b.Property<byte[]>("booking_time")
                        .IsRequired()
                        .HasColumnType("timestamp")
                        .HasColumnName("Booking_time");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("Status");

                    b.Property<int>("total")
                        .HasColumnType("int")
                        .HasColumnName("total");

                    b.Property<decimal?>("total_amout")
                        .HasColumnType("decimal")
                        .HasColumnName("Total_amout");

                    b.HasKey("Booking_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("Booking", (string)null);
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Event", b =>
                {
                    b.Property<int>("Event_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Event_ID"), 1L, 1);

                    b.Property<string>("Event_Name")
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Event_name");

                    b.Property<DateTime>("Event_date")
                        .HasColumnType("Date")
                        .HasColumnName("Event_date");

                    b.Property<DateTime>("Event_date_end")
                        .HasColumnType("Date")
                        .HasColumnName("Event_date_end");

                    b.Property<TimeSpan>("Event_time")
                        .HasColumnType("time")
                        .HasColumnName("Event_time");

                    b.Property<TimeSpan>("Event_time_end")
                        .HasColumnType("time")
                        .HasColumnName("Event_time_end");

                    b.Property<int?>("avaiable_ticket")
                        .HasColumnType("int")
                        .HasColumnName("Avaiable_tickets");

                    b.Property<byte[]>("created_at")
                        .IsRequired()
                        .HasColumnType("timestamp")
                        .HasColumnName("Created_at");

                    b.Property<DateTime>("created_at_time")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("Created_time")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<string>("event_image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Event_image");

                    b.Property<string>("location")
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Location");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Status");

                    b.Property<int?>("total_tickets")
                        .HasColumnType("int")
                        .HasColumnName("Total_tickets");

                    b.HasKey("Event_ID");

                    b.ToTable("Event", (string)null);
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Payment", b =>
                {
                    b.Property<int>("Payment_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Payment_ID"), 1L, 1);

                    b.Property<int?>("Booking_ID")
                        .HasColumnType("int");

                    b.Property<decimal?>("amount_paid")
                        .HasColumnType("decimal")
                        .HasColumnName("Amount_paid");

                    b.Property<byte[]>("payment_time")
                        .IsRequired()
                        .HasColumnType("timestamp")
                        .HasColumnName("Payment_time");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("Status");

                    b.HasKey("Payment_ID");

                    b.HasIndex("Booking_ID");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Report", b =>
                {
                    b.Property<int>("Report_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Report_ID"), 1L, 1);

                    b.Property<byte[]>("Created")
                        .IsRequired()
                        .HasColumnType("timestamp")
                        .HasColumnName("Created");

                    b.Property<int?>("Event_ID")
                        .HasColumnType("int");

                    b.Property<int?>("User_ID")
                        .HasColumnType("int");

                    b.Property<string>("comment")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Comment");

                    b.Property<int?>("rate")
                        .HasColumnType("int")
                        .HasColumnName("Report");

                    b.HasKey("Report_ID");

                    b.HasIndex("Event_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("Report", (string)null);
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Ticket", b =>
                {
                    b.Property<int>("Ticket_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Ticket_ID"), 1L, 1);

                    b.Property<int?>("Event_ID")
                        .HasColumnType("int");

                    b.Property<string>("Ticket_type")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Ticket_type");

                    b.Property<decimal?>("price")
                        .HasColumnType("Decimal(10,2)")
                        .HasColumnName("Price");

                    b.Property<int?>("seat_number")
                        .HasColumnType("int")
                        .HasColumnName("Seat_number");

                    b.Property<int?>("seat_remain")
                        .HasColumnType("int")
                        .HasColumnName("Seat_remain");

                    b.Property<DateTime>("start_time")
                        .HasColumnType("datetime2")
                        .HasColumnName("start_time");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Status");

                    b.HasKey("Ticket_ID");

                    b.HasIndex("Event_ID");

                    b.ToTable("Ticket", (string)null);
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Address");

                    b.Property<byte[]>("Created_at")
                        .IsRequired()
                        .HasColumnType("timestamp")
                        .HasColumnName("Created_at");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Email");

                    b.Property<Guid?>("EmailVerificationTokenId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Password");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("Phone");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name");

                    b.Property<string>("UserSurname")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Surname");

                    b.Property<string>("avatarImg")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AvatarImgUrl");

                    b.Property<DateTime?>("birthday")
                        .HasColumnType("Date")
                        .HasColumnName("Birthday");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("Gender");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("Role");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("Status");

                    b.HasKey("UserID");

                    b.HasIndex("EmailVerificationTokenId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Do_an_ticket_box.Services.EmailVerificationToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EmailVerificationTokens", (string)null);
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Booking", b =>
                {
                    b.HasOne("Do_an_ticket_box.Models.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("User_ID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Payment", b =>
                {
                    b.HasOne("Do_an_ticket_box.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("Booking_ID");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Report", b =>
                {
                    b.HasOne("Do_an_ticket_box.Models.Event", "Event")
                        .WithMany("Report")
                        .HasForeignKey("Event_ID");

                    b.HasOne("Do_an_ticket_box.Models.User", "User")
                        .WithMany("Reports")
                        .HasForeignKey("User_ID");

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Ticket", b =>
                {
                    b.HasOne("Do_an_ticket_box.Models.Event", "Event")
                        .WithMany("Ticket")
                        .HasForeignKey("Event_ID");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.User", b =>
                {
                    b.HasOne("Do_an_ticket_box.Services.EmailVerificationToken", null)
                        .WithMany("User")
                        .HasForeignKey("EmailVerificationTokenId");
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.Event", b =>
                {
                    b.Navigation("Report");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("Do_an_ticket_box.Models.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Do_an_ticket_box.Services.EmailVerificationToken", b =>
                {
                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
