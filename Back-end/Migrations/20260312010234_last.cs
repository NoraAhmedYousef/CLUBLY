using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "ActivityGroups");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ActivityGroups");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "ActivityGroupTimeSlots",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "ActivityGroupTimeSlots");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "ActivityGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "ActivityGroups",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
