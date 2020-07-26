using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class MovieYearAndAverageRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "average_rating",
                table: "movies",
                type: "decimal(2, 1)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "year",
                table: "movies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "average_rating",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "year",
                table: "movies");
        }
    }
}
