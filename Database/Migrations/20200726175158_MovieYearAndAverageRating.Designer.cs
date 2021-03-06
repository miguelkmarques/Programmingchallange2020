﻿// <auto-generated />
using System;
using Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200726175158_MovieYearAndAverageRating")]
    partial class MovieYearAndAverageRating
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Database.Models.GenomeScores", b =>
                {
                    b.Property<long>("MovieId")
                        .HasColumnName("movie_id")
                        .HasColumnType("bigint");

                    b.Property<long>("TagId")
                        .HasColumnName("tag_id")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Relevance")
                        .HasColumnName("relevance")
                        .HasColumnType("decimal(20,19)");

                    b.HasKey("MovieId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("genome_scores");
                });

            modelBuilder.Entity("Database.Models.GenomeTags", b =>
                {
                    b.Property<long>("TagId")
                        .HasColumnName("tag_id")
                        .HasColumnType("bigint");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnName("tag")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.HasKey("TagId");

                    b.ToTable("genome_tags");
                });

            modelBuilder.Entity("Database.Models.Genres", b =>
                {
                    b.Property<string>("Genre")
                        .HasColumnName("genre")
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.HasKey("Genre");

                    b.ToTable("genres");
                });

            modelBuilder.Entity("Database.Models.Links", b =>
                {
                    b.Property<long>("MovieId")
                        .HasColumnName("movie_id")
                        .HasColumnType("bigint");

                    b.Property<string>("ImdbId")
                        .HasColumnName("imdb_id")
                        .HasColumnType("character varying(7)")
                        .HasMaxLength(7);

                    b.Property<long?>("TmdbId")
                        .HasColumnName("tmdb_id")
                        .HasColumnType("bigint");

                    b.HasKey("MovieId");

                    b.ToTable("links");
                });

            modelBuilder.Entity("Database.Models.MovieGenres", b =>
                {
                    b.Property<long>("MovieId")
                        .HasColumnName("movie_id")
                        .HasColumnType("bigint");

                    b.Property<string>("Genre")
                        .HasColumnName("genre")
                        .HasColumnType("character varying(20)");

                    b.HasKey("MovieId", "Genre");

                    b.HasIndex("Genre");

                    b.ToTable("movie_genres");
                });

            modelBuilder.Entity("Database.Models.Movies", b =>
                {
                    b.Property<long>("MovieId")
                        .HasColumnName("movie_id")
                        .HasColumnType("bigint");

                    b.Property<decimal>("AverageRating")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("average_rating")
                        .HasColumnType("decimal(2, 1)")
                        .HasDefaultValue(0m);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<int>("Year")
                        .HasColumnName("year")
                        .HasColumnType("integer");

                    b.HasKey("MovieId");

                    b.ToTable("movies");
                });

            modelBuilder.Entity("Database.Models.Ratings", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.Property<long>("MovieId")
                        .HasColumnName("movie_id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnName("date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Rating")
                        .HasColumnName("rating")
                        .HasColumnType("decimal(2, 1)");

                    b.HasKey("UserId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("ratings");
                });

            modelBuilder.Entity("Database.Models.Tags", b =>
                {
                    b.Property<long>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("tag_id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnName("date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("MovieId")
                        .HasColumnName("movie_id")
                        .HasColumnType("bigint");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnName("tag")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("TagId");

                    b.HasIndex("MovieId");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("Database.Models.GenomeScores", b =>
                {
                    b.HasOne("Database.Models.Movies", "Movie")
                        .WithMany("GenomeScores")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("fk_genome_scores_movies_movie_temp_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.GenomeTags", "GenomeTag")
                        .WithMany("GenomeScores")
                        .HasForeignKey("TagId")
                        .HasConstraintName("fk_genome_scores_genome_tags_genome_tag_temp_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.Links", b =>
                {
                    b.HasOne("Database.Models.Movies", "Movie")
                        .WithOne("Links")
                        .HasForeignKey("Database.Models.Links", "MovieId")
                        .HasConstraintName("fk_links_movies_movie_temp_id1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.MovieGenres", b =>
                {
                    b.HasOne("Database.Models.Genres", "GenreNav")
                        .WithMany("Movies")
                        .HasForeignKey("Genre")
                        .HasConstraintName("fk_movie_genres_genres_genre_nav_temp_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.Movies", "Movie")
                        .WithMany("Genres")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("fk_movie_genres_movies_movie_temp_id2")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.Ratings", b =>
                {
                    b.HasOne("Database.Models.Movies", "Movie")
                        .WithMany("Ratings")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("fk_ratings_movies_movie_temp_id3")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.Tags", b =>
                {
                    b.HasOne("Database.Models.Movies", "Movie")
                        .WithMany("Tags")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("fk_tags_movies_movie_temp_id4")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
