using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartMeal.Data.Migrations
{
    public partial class Ingredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "Ingredient",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Metric",
                table: "Ingredient",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Metric",
                table: "Ingredient");
        }
    }
}
