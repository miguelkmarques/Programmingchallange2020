using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class GenomeTagsAndScores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenomeTags",
                columns: table => new
                {
                    TagId = table.Column<long>(nullable: false),
                    Tag = table.Column<string>(unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenomeTags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "GenomeScores",
                columns: table => new
                {
                    MovieId = table.Column<long>(nullable: false),
                    TagId = table.Column<long>(nullable: false),
                    Relevance = table.Column<decimal>(type: "decimal(19,19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenomeScores", x => new { x.MovieId, x.TagId });
                    table.ForeignKey(
                        name: "FK_GenomeScores_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenomeScores_GenomeTags_TagId",
                        column: x => x.TagId,
                        principalTable: "GenomeTags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenomeScores_TagId",
                table: "GenomeScores",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenomeScores");

            migrationBuilder.DropTable(
                name: "GenomeTags");
        }
    }
}
