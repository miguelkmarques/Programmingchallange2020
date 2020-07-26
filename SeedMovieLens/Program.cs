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
            finally
            {
                Context.Dispose();
            }
            Logger.Warn("end");
        }

        private static void SeedData()
        {
            Context.Database.Migrate();
            InsertMoviesAndGenres();
            InsertRatings();
        }

        private static void InsertMoviesAndGenres()
        {
            var moviesService = new MoviesService();
            moviesService.ReadCsvFile(Path.Combine(Configuration.GetSection("CsvPath").Value, "movies.csv"), Configuration.GetConnectionString("DefaultConnection"), 1);
        }

        private static void InsertRatings()
        {
            var ratingsService = new RatingsService();
            ratingsService.ReadCsvFile(Path.Combine(Configuration.GetSection("CsvPath").Value, "ratings.csv"), Configuration.GetConnectionString("DefaultConnection"), 4);
        }
    }
}
