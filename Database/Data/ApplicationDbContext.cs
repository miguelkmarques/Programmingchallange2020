using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Database.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Extensions;

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
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                // Convert column names from PascalCase to snake_case.
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }

                // Convert primary key names from PascalCase to snake_case. E.g. PK_users -> pk_users
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                // Convert foreign key names from PascalCase to snake_case.
                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                // Convert index names from PascalCase to snake_case.
                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToSnakeCase());
                }
            }

            builder.Entity<Movies>(entity =>
            {
                entity.HasKey(c => c.MovieId);

                entity.Property(e => e.MovieId).ValueGeneratedNever();

                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);

                entity.Property(e => e.AverageRating).HasColumnType("decimal(2, 1)").HasDefaultValue(0);

                entity.HasMany(e => e.Genres).WithOne(e => e.Movie).HasForeignKey(e => e.MovieId);

                entity.HasMany(e => e.Ratings).WithOne(e => e.Movie).HasForeignKey(e => e.MovieId);

                entity.HasOne(e => e.Links).WithOne(e => e.Movie);

                entity.HasMany(e => e.Tags).WithOne(e => e.Movie).HasForeignKey(e => e.MovieId);
            });

            builder.Entity<Genres>(entity =>
            {
                entity.HasKey(c => c.Genre);

                entity.Property(e => e.Genre).HasMaxLength(20);

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

                entity.Property(e => e.ImdbId).HasMaxLength(8);
            });

            builder.Entity<Tags>(entity =>
            {
                entity.HasKey(c => c.TagId);

                entity.Property(e => e.Tag).HasMaxLength(255).IsRequired();
            });

            builder.Entity<GenomeTags>(entity =>
            {
                entity.HasKey(c => c.TagId);

                entity.Property(e => e.TagId).ValueGeneratedNever();

                entity.Property(e => e.Tag).HasMaxLength(255).IsRequired();

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
