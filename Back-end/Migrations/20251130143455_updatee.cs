using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class updatee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroups_Facilities_FacilityId",
                table: "ActivityGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroups_Facilities_FacilityId1",
                table: "ActivityGroups");

            migrationBuilder.DropIndex(
                name: "IX_ActivityGroups_FacilityId1",
                table: "ActivityGroups");

            migrationBuilder.DropColumn(
                name: "FacilityId1",
                table: "ActivityGroups");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityId",
                table: "ActivityGroups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGroups_Facilities_FacilityId",
                table: "ActivityGroups",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroups_Facilities_FacilityId",
                table: "ActivityGroups");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityId",
                table: "ActivityGroups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacilityId1",
                table: "ActivityGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroups_FacilityId1",
                table: "ActivityGroups",
                column: "FacilityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGroups_Facilities_FacilityId",
                table: "ActivityGroups",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGroups_Facilities_FacilityId1",
                table: "ActivityGroups",
                column: "FacilityId1",
                principalTable: "Facilities",
                principalColumn: "Id");
        }
    }
}
