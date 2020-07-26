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
            //Context.Database.Migrate();
            //InsertMoviesAndGenres();
            //InsertRatings();
            //InsertAverageRatingOfMovies();
            //InsertLinks();
            //InsertTags();
            InsertGenomeTags();
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

        private static void InsertLinks()
        {
            var linksService = new LinksService();
            linksService.ReadCsvFile(Path.Combine(Configuration.GetSection("CsvPath").Value, "links.csv"), Configuration.GetConnectionString("DefaultConnection"), 1);
        }

        private static void InsertTags()
        {
            var tagsService = new TagsService();
            tagsService.ReadCsvFile(Path.Combine(Configuration.GetSection("CsvPath").Value, "tags.csv"), Configuration.GetConnectionString("DefaultConnection"), 2);
        }

        private static void InsertGenomeTags()
        {
            var genometagsService = new GenomeTagsService();
            genometagsService.ReadCsvFile(Path.Combine(Configuration.GetSection("CsvPath").Value, "genome-tags.csv"), Configuration.GetConnectionString("DefaultConnection"), 1);
        }

        private static void InsertAverageRatingOfMovies()
        {
            Logger.Info("Starting updating average rating of Movies");
            Context.Database.ExecuteSqlRaw(@"update movies 
set average_rating = a.rating
from(select r.movie_id, avg(r.rating) rating
from ratings r
group by r.movie_id
) a where movies.movie_id = a.movie_id");
            Logger.Info("Average rating updated");
        }
    }
}
