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

        /// <summary>
        /// Aplicar as Migrations do banco de dados e popular o banco de dados com os arquivos CSV
        /// </summary>
        private static void SeedData()
        {
            Context.Database.Migrate();

            var csvPath = Configuration.GetSection("CsvPath").Value;
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            InsertMoviesAndGenres(csvPath, connectionString);
            InsertRatings(csvPath, connectionString);
            InsertAverageRatingOfMovies();
            InsertLinks(csvPath, connectionString);
            InsertTags(csvPath, connectionString);
            InsertGenomeTags(csvPath, connectionString);
            InsertGenomeScores(csvPath, connectionString);
        }


        /// <summary>
        /// Popular as Tabelas Movies e MovieGenres
        /// </summary>
        /// <param name="csvPath">Caminho raiz de onde se encontra os arquivos CSV</param>
        /// <param name="connectionString">Connection String para acessar o banco de dados</param>
        private static void InsertMoviesAndGenres(string csvPath, string connectionString)
        {
            var moviesService = new MoviesService();
            moviesService.ReadCsvFile(Path.Combine(csvPath, "movies.csv"), connectionString, 1);
        }

        /// <summary>
        /// Popular a Tabela Ratings
        /// </summary>
        /// <param name="csvPath">Caminho raiz de onde se encontra os arquivos CSV</param>
        /// <param name="connectionString">Connection String para acessar o banco de dados</param>
        private static void InsertRatings(string csvPath, string connectionString)
        {
            var ratingsService = new RatingsService();
            ratingsService.ReadCsvFile(Path.Combine(csvPath, "ratings.csv"), connectionString, 4);
        }

        /// <summary>
        /// Popular a Tabela Links
        /// </summary>
        /// <param name="csvPath">Caminho raiz de onde se encontra os arquivos CSV</param>
        /// <param name="connectionString">Connection String para acessar o banco de dados</param>
        private static void InsertLinks(string csvPath, string connectionString)
        {
            var linksService = new LinksService();
            linksService.ReadCsvFile(Path.Combine(csvPath, "links.csv"), connectionString, 1);
        }

        /// <summary>
        /// Popular a Tabela Tags
        /// </summary>
        /// <param name="csvPath">Caminho raiz de onde se encontra os arquivos CSV</param>
        /// <param name="connectionString">Connection String para acessar o banco de dados</param>
        private static void InsertTags(string csvPath, string connectionString)
        {
            var tagsService = new TagsService();
            tagsService.ReadCsvFile(Path.Combine(csvPath, "tags.csv"), connectionString, 2);
        }

        /// <summary>
        /// Popular a Tabela GenomeTags
        /// </summary>
        /// <param name="csvPath">Caminho raiz de onde se encontra os arquivos CSV</param>
        /// <param name="connectionString">Connection String para acessar o banco de dados</param>
        private static void InsertGenomeTags(string csvPath, string connectionString)
        {
            var genometagsService = new GenomeTagsService();
            genometagsService.ReadCsvFile(Path.Combine(csvPath, "genome-tags.csv"), connectionString, 1);
        }
        /// <summary>
        /// Popular a Tabela GenomeScores
        /// </summary>
        /// <param name="csvPath">Caminho raiz de onde se encontra os arquivos CSV</param>
        /// <param name="connectionString">Connection String para acessar o banco de dados</param>
        private static void InsertGenomeScores(string csvPath, string connectionString)
        {
            var genomeScoresService = new GenomeScoresService();
            genomeScoresService.ReadCsvFile(Path.Combine(csvPath, "genome-scores.csv"), connectionString, 4);
        }

        /// <summary>
        /// Rodar um Script SQL para atualizar o Average Score de todos os Movies com base na Tabela Ratings
        /// </summary>
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
