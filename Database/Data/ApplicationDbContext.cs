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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Movies>(entity =>
            {
                entity.HasKey(c => c.MovieId);

                entity.Property(e => e.MovieId).ValueGeneratedNever();

                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);

                entity.HasMany(e => e.Genres).WithOne(e => e.Movie).HasForeignKey(e => e.MovieId);
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
        }
    }
}
