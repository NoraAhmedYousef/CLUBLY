using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class bookingupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityBookings_Members_MemberId",
                table: "ActivityBookings");

            migrationBuilder.AddColumn<int>(
                name: "GuestId",
                table: "FacilityBookings",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "ActivityBookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BookedByEmail",
                table: "ActivityBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookedByName",
                table: "ActivityBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_GuestId",
                table: "FacilityBookings",
                column: "GuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityBookings_Members_MemberId",
                table: "ActivityBookings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityBookings_Guests_GuestId",
                table: "FacilityBookings",
                column: "GuestId",
                principalTable: "Guests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityBookings_Members_MemberId",
                table: "ActivityBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityBookings_Guests_GuestId",
                table: "FacilityBookings");

            migrationBuilder.DropIndex(
                name: "IX_FacilityBookings_GuestId",
                table: "FacilityBookings");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "FacilityBookings");

            migrationBuilder.DropColumn(
                name: "BookedByEmail",
                table: "ActivityBookings");

            migrationBuilder.DropColumn(
                name: "BookedByName",
                table: "ActivityBookings");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "ActivityBookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityBookings_Members_MemberId",
                table: "ActivityBookings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
