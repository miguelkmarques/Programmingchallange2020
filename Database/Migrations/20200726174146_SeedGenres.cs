using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SeedGenres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into genres values ('Adventure')");
            migrationBuilder.Sql("insert into genres values ('Western')");
            migrationBuilder.Sql("insert into genres values ('Musical')");
            migrationBuilder.Sql("insert into genres values ('War')");
            migrationBuilder.Sql("insert into genres values ('Documentary')");
            migrationBuilder.Sql("insert into genres values ('IMAX')");
            migrationBuilder.Sql("insert into genres values ('Sci-Fi')");
            migrationBuilder.Sql("insert into genres values ('Mystery')");
            migrationBuilder.Sql("insert into genres values ('Horror')");
            migrationBuilder.Sql("insert into genres values ('Thriller')");
            migrationBuilder.Sql("insert into genres values ('Crime')");
            migrationBuilder.Sql("insert into genres values ('Action')");
            migrationBuilder.Sql("insert into genres values ('Drama')");
            migrationBuilder.Sql("insert into genres values ('Romance')");
            migrationBuilder.Sql("insert into genres values ('Fantasy')");
            migrationBuilder.Sql("insert into genres values ('Comedy')");
            migrationBuilder.Sql("insert into genres values ('Children')");
            migrationBuilder.Sql("insert into genres values ('Animation')");
            migrationBuilder.Sql("insert into genres values ('Film-Noir')");
            migrationBuilder.Sql("insert into genres values ('(no genres listed)')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from genres");
        }
    }
}
