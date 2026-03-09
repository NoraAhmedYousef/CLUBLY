using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerToActivityGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "ActivityGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroups_TrainerId",
                table: "ActivityGroups",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGroups_Trainers_TrainerId",
                table: "ActivityGroups",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroups_Trainers_TrainerId",
                table: "ActivityGroups");

            migrationBuilder.DropIndex(
                name: "IX_ActivityGroups_TrainerId",
                table: "ActivityGroups");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "ActivityGroups");
        }
    }
}
