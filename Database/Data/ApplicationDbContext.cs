using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Database.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movies> Movies { get; set; }

        public DbSet<Genres> Genres { get; set; }

        public DbSet<MovieGenres> MovieGenres { get; set; }

        public DbSet<Ratings> Ratings { get; set; }

        public DbSet<Links> Links { get; set; }

        public DbSet<Tags> Tags { get; set; }

        public DbSet<GenomeTags> GenomeTags { get; set; }

        public DbSet<GenomeScores> GenomeScores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Movies>(entity =>
            {
                entity.HasKey(c => c.MovieId);

                entity.Property(e => e.MovieId).ValueGeneratedNever();

                entity.Property(e => e.Title).IsRequired().HasMaxLength(255).IsUnicode(false);

                entity.HasMany(e => e.Genres).WithOne(e => e.Movie).HasForeignKey(e => e.MovieId);

                entity.HasMany(e => e.Ratings).WithOne(e => e.Movie).HasForeignKey(e => e.MovieId);

                entity.HasOne(e => e.Links).WithOne(e => e.Movie);

                entity.HasMany(e => e.Tags).WithOne(e => e.Movie).HasForeignKey(e => e.MovieId);
            });

            builder.Entity<Genres>(entity =>
            {
                entity.HasKey(c => c.Genre);

                entity.Property(e => e.Genre).HasMaxLength(20).IsUnicode(false);

                entity.HasMany(e => e.Movies).WithOne(e => e.GenreNav).HasForeignKey(e => e.Genre);
            });

            builder.Entity<MovieGenres>(entity =>
            {
                entity.HasKey(c => new { c.MovieId, c.Genre });
            });

            builder.Entity<Ratings>(entity =>
            {
                entity.HasKey(c => new { c.UserId, c.MovieId });

                entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            });

            builder.Entity<Links>(entity =>
            {
                entity.HasKey(c => c.MovieId);

                entity.Property(e => e.ImdbId).HasMaxLength(7).IsUnicode(false);
            });

            builder.Entity<Tags>(entity =>
            {
                entity.HasKey(c => c.TagId);

                entity.Property(e => e.Tag).HasMaxLength(255).IsRequired().IsUnicode(false);
            });

            builder.Entity<GenomeTags>(entity =>
            {
                entity.HasKey(c => c.TagId);

                entity.Property(e => e.TagId).ValueGeneratedNever();

                entity.Property(e => e.Tag).HasMaxLength(255).IsRequired().IsUnicode(false);

                entity.HasMany(e => e.GenomeScores).WithOne(e => e.GenomeTag).HasForeignKey(e => e.TagId);
            });

            builder.Entity<GenomeScores>(entity =>
            {
                entity.HasKey(c => new { c.MovieId, c.TagId });

                entity.Property(e => e.Relevance).HasColumnType("decimal(20,19)");
            });
        }
    }
}
