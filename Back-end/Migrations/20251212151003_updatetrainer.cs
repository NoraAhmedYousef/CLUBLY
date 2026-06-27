using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class updatetrainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Activities_ActivityId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_ActivityId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Activities",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Trainers");

            migrationBuilder.CreateTable(
                name: "ActivityTrainer",
                columns: table => new
                {
                    ActivitiesId = table.Column<int>(type: "int", nullable: false),
                    TrainersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTrainer", x => new { x.ActivitiesId, x.TrainersId });
                    table.ForeignKey(
                        name: "FK_ActivityTrainer_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityTrainer_Trainers_TrainersId",
                        column: x => x.TrainersId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTrainer_TrainersId",
                table: "ActivityTrainer",
                column: "TrainersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityTrainer");

            migrationBuilder.AddColumn<string>(
                name: "Activities",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

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
    }
}
