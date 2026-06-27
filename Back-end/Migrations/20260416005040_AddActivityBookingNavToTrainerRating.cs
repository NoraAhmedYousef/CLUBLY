using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityBookingNavToTrainerRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TrainerRatings_ActivityBookingId",
                table: "TrainerRatings",
                column: "ActivityBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerRatings_ActivityBookings_ActivityBookingId",
                table: "TrainerRatings",
                column: "ActivityBookingId",
                principalTable: "ActivityBookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainerRatings_ActivityBookings_ActivityBookingId",
                table: "TrainerRatings");

            migrationBuilder.DropIndex(
                name: "IX_TrainerRatings_ActivityBookingId",
                table: "TrainerRatings");
        }
    }
}
