using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartMeal.Data.Migrations
{
    public partial class RecipeImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Recipe");

            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "Recipe",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_ImageId",
                table: "Recipe",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Photos_ImageId",
                table: "Recipe",
                column: "ImageId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Photos_ImageId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_ImageId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Recipe");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Recipe",
                nullable: true);
        }
    }
}
