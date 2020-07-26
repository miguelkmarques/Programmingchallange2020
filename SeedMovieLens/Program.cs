using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SeedMovieLens.CsvService;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SeedMovieLens
{
    class Program
    {
        private static readonly DbContextOptionsBuilder<ApplicationDbContext> OptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        public static ApplicationDbContext Context;
        private static readonly IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logger.Warn("start");
            OptionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            //var loggerFactory = LoggerFactory.Create(configure => { configure.AddNLog(); });
            //OptionsBuilder.UseLoggerFactory(loggerFactory);
            Context = new ApplicationDbContext(OptionsBuilder.Options);
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                SeedData();
            }
            catch (Exception err)
            {
                Logger.Error(err);
            }

            Logger.Warn("end");
        }

        private static void SeedData()
        {
            Context.Database.Migrate();

            //InsertMoviesAndGenres();
            InsertRatings();
        }

        private static void InsertMoviesAndGenres()
        {
            var moviesService = new MoviesService();
            var movies = moviesService.ReadCsvFile(Path.Combine(Configuration.GetSection("CsvPath").Value, "movies.csv"));
            var genres = movies.SelectMany(s => s.Genres).Select(s => s.Genre).Distinct().Select(s => new Genres { Genre = s });


            Logger.Info($"Inserting {genres.Count()} genres");
            Context.Genres.AddRange(genres);
            Context.SaveChanges();
            Logger.Info($"Genres inserted");
            Logger.Info($"Inserting {movies.Count()} movies");
            Context.Movies.AddRange(movies);
            Context.SaveChanges();
            Logger.Info($"Movies inserted");
            
        }

        private static void InsertRatings()
        {
            var ratingsService = new RatingsService();
            ratingsService.ReadCsvFile(Path.Combine(Configuration.GetSection("CsvPath").Value, "ratings.csv"), Context, Configuration.GetConnectionString("DefaultConnection"));
            //var ratings = ratingsService.ReadCsvFile(Path.Combine(Configuration.GetSection("CsvPath").Value, "ratings.csv"));
            //Logger.Info($"Inserting {ratings.Count()} genres");
            //Context.Entry(ratings).State = EntityState.Added;
            //Context.SaveChanges();
            Logger.Info($"Ratings inserted");
        }
    }
}
