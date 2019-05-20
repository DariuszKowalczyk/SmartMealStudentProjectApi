using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartMeal.Data.Migrations
{
    public partial class UserCreatedby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Recipe",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UploadById",
                table: "Photos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_CreatedById",
                table: "Recipe",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CreatedById",
                table: "Product",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UploadById",
                table: "Photos",
                column: "UploadById");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_User_UploadById",
                table: "Photos",
                column: "UploadById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_User_CreatedById",
                table: "Product",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_User_CreatedById",
                table: "Recipe",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_User_UploadById",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_User_CreatedById",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_User_CreatedById",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_CreatedById",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Product_CreatedById",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Photos_UploadById",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UploadById",
                table: "Photos");
        }
    }
}
