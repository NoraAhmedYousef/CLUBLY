using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberNavToFacilityBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_MemberId",
                table: "FacilityBookings",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityBookings_Members_MemberId",
                table: "FacilityBookings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityBookings_Members_MemberId",
                table: "FacilityBookings");

            migrationBuilder.DropIndex(
                name: "IX_FacilityBookings_MemberId",
                table: "FacilityBookings");
        }
    }
}
