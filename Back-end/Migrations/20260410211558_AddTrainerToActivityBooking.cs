using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerToActivityBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "ActivityBookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityBookings_TrainerId",
                table: "ActivityBookings",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityBookings_Trainers_TrainerId",
                table: "ActivityBookings",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityBookings_Trainers_TrainerId",
                table: "ActivityBookings");

            migrationBuilder.DropIndex(
                name: "IX_ActivityBookings_TrainerId",
                table: "ActivityBookings");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "ActivityBookings");
        }
    }
}
