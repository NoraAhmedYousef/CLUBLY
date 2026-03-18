using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUp.Migrations
{
    /// <inheritdoc />
    public partial class updateannauncmen3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForWho",
                table: "Announcements",
                newName: "ForWhoRaw");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForWhoRaw",
                table: "Announcements",
                newName: "ForWho");
        }
    }
}
