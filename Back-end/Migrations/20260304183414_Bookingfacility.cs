using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class Bookingfacility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilitySchedules_Facilities_FacilityId",
                table: "FacilitySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityTimeSlots_FacilitySchedules_FacilityScheduleId",
                table: "FacilityTimeSlots");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.CreateTable(
                name: "FacilityBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    FacilityScheduleId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    BookedByName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BookedByEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Participants = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceiptImageUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityBookings_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityBookings_FacilitySchedules_FacilityScheduleId",
                        column: x => x.FacilityScheduleId,
                        principalTable: "FacilitySchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_FacilityId",
                table: "FacilityBookings",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_FacilityScheduleId",
                table: "FacilityBookings",
                column: "FacilityScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilitySchedules_Facilities_FacilityId",
                table: "FacilitySchedules",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityTimeSlots_FacilitySchedules_FacilityScheduleId",
                table: "FacilityTimeSlots",
                column: "FacilityScheduleId",
                principalTable: "FacilitySchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilitySchedules_Facilities_FacilityId",
                table: "FacilitySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityTimeSlots_FacilitySchedules_FacilityScheduleId",
                table: "FacilityTimeSlots");

            migrationBuilder.DropTable(
                name: "FacilityBookings");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityGroupId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfParticipants = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_ActivityGroups_ActivityGroupId",
                        column: x => x.ActivityGroupId,
                        principalTable: "ActivityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ActivityGroupId",
                table: "Bookings",
                column: "ActivityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ActivityId",
                table: "Bookings",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberId",
                table: "Bookings",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TrainerId",
                table: "Bookings",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilitySchedules_Facilities_FacilityId",
                table: "FacilitySchedules",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityTimeSlots_FacilitySchedules_FacilityScheduleId",
                table: "FacilityTimeSlots",
                column: "FacilityScheduleId",
                principalTable: "FacilitySchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
