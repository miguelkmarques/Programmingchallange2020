using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class FixingDecimalPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Ratings",
                type: "decimal(2, 1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(1, 1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Relevance",
                table: "GenomeScores",
                type: "decimal(20,19)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,19)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Ratings",
                type: "decimal(1, 1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2, 1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Relevance",
                table: "GenomeScores",
                type: "decimal(19,19)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,19)");
        }
    }
}
