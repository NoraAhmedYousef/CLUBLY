using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class trainer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Trainers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_ActivityId",
                table: "Trainers",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Activities_ActivityId",
                table: "Trainers",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Activities_ActivityId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_ActivityId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Trainers");
        }
    }
}
