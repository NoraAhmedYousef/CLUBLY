using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class d : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId1",
                table: "ActivityGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacilityId1",
                table: "ActivityGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroups_ActivityId1",
                table: "ActivityGroups",
                column: "ActivityId1");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroups_FacilityId1",
                table: "ActivityGroups",
                column: "FacilityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGroups_Activities_ActivityId1",
                table: "ActivityGroups",
                column: "ActivityId1",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGroups_Facilities_FacilityId1",
                table: "ActivityGroups",
                column: "FacilityId1",
                principalTable: "Facilities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroups_Activities_ActivityId1",
                table: "ActivityGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroups_Facilities_FacilityId1",
                table: "ActivityGroups");

            migrationBuilder.DropIndex(
                name: "IX_ActivityGroups_ActivityId1",
                table: "ActivityGroups");

            migrationBuilder.DropIndex(
                name: "IX_ActivityGroups_FacilityId1",
                table: "ActivityGroups");

            migrationBuilder.DropColumn(
                name: "ActivityId1",
                table: "ActivityGroups");

            migrationBuilder.DropColumn(
                name: "FacilityId1",
                table: "ActivityGroups");
        }
    }
}
