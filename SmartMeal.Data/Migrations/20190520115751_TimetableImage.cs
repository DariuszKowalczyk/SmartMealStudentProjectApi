using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartMeal.Data.Migrations
{
    public partial class TimetableImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "Timetables",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_OwnerId",
                table: "Timetables",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_User_OwnerId",
                table: "Timetables",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_User_OwnerId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_OwnerId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Timetables");
        }
    }
}
