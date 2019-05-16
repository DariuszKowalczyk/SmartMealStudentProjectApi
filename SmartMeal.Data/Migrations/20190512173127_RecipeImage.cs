using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartMeal.Data.Migrations
{
    public partial class RecipeImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Recipe",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Recipe");
        }
    }
}
