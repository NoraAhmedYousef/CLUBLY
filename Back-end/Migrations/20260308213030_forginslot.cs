using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class forginslot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityTimeSlots_FacilitySchedules_FacilityScheduleId",
                table: "FacilityTimeSlots");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityTimeSlots_FacilitySchedules_FacilityScheduleId",
                table: "FacilityTimeSlots",
                column: "FacilityScheduleId",
                principalTable: "FacilitySchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityTimeSlots_FacilitySchedules_FacilityScheduleId",
                table: "FacilityTimeSlots");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityTimeSlots_FacilitySchedules_FacilityScheduleId",
                table: "FacilityTimeSlots",
                column: "FacilityScheduleId",
                principalTable: "FacilitySchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
