using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genome_tags",
                columns: table => new
                {
                    tag_id = table.Column<long>(nullable: false),
                    tag = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genome_tags", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    genre = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.genre);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    movie_id = table.Column<long>(nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movies", x => x.movie_id);
                });

            migrationBuilder.CreateTable(
                name: "genome_scores",
                columns: table => new
                {
                    movie_id = table.Column<long>(nullable: false),
                    tag_id = table.Column<long>(nullable: false),
                    relevance = table.Column<decimal>(type: "decimal(20,19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genome_scores", x => new { x.movie_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_genome_scores_movies_movie_temp_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_genome_scores_genome_tags_genome_tag_temp_id",
                        column: x => x.tag_id,
                        principalTable: "genome_tags",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "links",
                columns: table => new
                {
                    movie_id = table.Column<long>(nullable: false),
                    imdb_id = table.Column<string>(maxLength: 7, nullable: true),
                    tmdb_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_links", x => x.movie_id);
                    table.ForeignKey(
                        name: "fk_links_movies_movie_temp_id1",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movie_genres",
                columns: table => new
                {
                    movie_id = table.Column<long>(nullable: false),
                    genre = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_genres", x => new { x.movie_id, x.genre });
                    table.ForeignKey(
                        name: "fk_movie_genres_genres_genre_nav_temp_id",
                        column: x => x.genre,
                        principalTable: "genres",
                        principalColumn: "genre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movie_genres_movies_movie_temp_id2",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    user_id = table.Column<long>(nullable: false),
                    movie_id = table.Column<long>(nullable: false),
                    rating = table.Column<decimal>(type: "decimal(2, 1)", nullable: false),
                    date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ratings", x => new { x.user_id, x.movie_id });
                    table.ForeignKey(
                        name: "fk_ratings_movies_movie_temp_id3",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tag_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(nullable: false),
                    movie_id = table.Column<long>(nullable: false),
                    tag = table.Column<string>(maxLength: 255, nullable: false),
                    date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.tag_id);
                    table.ForeignKey(
                        name: "fk_tags_movies_movie_temp_id4",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_genome_scores_tag_id",
                table: "genome_scores",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_movie_genres_genre",
                table: "movie_genres",
                column: "genre");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_movie_id",
                table: "ratings",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_tags_movie_id",
                table: "tags",
                column: "movie_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "genome_scores");

            migrationBuilder.DropTable(
                name: "links");

            migrationBuilder.DropTable(
                name: "movie_genres");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "genome_tags");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "movies");
        }
    }
}
