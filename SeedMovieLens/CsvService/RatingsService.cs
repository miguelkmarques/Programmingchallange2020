using CsvHelper;
using CsvHelper.Configuration;
using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedMovieLens.CsvService
{
    public class RatingsService
    {
        private readonly DbContextOptionsBuilder<ApplicationDbContext> OptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void ReadCsvFile(string location, string connectionString, int threads)
        {
            OptionsBuilder.UseNpgsql(connectionString);
            try
            {
                int countRecords = 0;
                using (var reader = new StreamReader(location, Encoding.UTF8))
                {
                    while (reader.ReadLine() != null)
                    {
                        countRecords++;
                    }
                    countRecords--;
                }
                Logger.Info($"Inserting {countRecords} ratings using {threads} threads");
                int divisao = countRecords / threads;

                List<Task> tasks = new List<Task>();

                for (int i = 0; i < threads; i++)
                {
                    int skip = divisao * i;
                    int take = countRecords - divisao * i;
                    if (i == threads - 1)
                    {
                        tasks.Add(Task.Run(() => StartThread(location, new ApplicationDbContext(OptionsBuilder.Options), skip, take)));
                    }
                    else
                    {
                        tasks.Add(Task.Run(() => StartThread(location, new ApplicationDbContext(OptionsBuilder.Options), skip, divisao)));
                    }
                }


                Task.WaitAll(tasks.ToArray());
                Logger.Info("Ratings inserted");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void StartThread(string location, DbContext context, int skip, int take)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                using var reader = new StreamReader(location, Encoding.UTF8);

                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Configuration.RegisterClassMap<RatingMap>();
                int count = 0;
                foreach (var r in csv.GetRecords<Ratings>().Skip(skip).Take(take))
                {
                    context.Add(r);
                    if (count == 1000)
                    {
                        context.SaveChanges();
                        DetachEntries(context);
                        count = 0;
                    }
                    count++;
                }
                context.SaveChanges();
                DetachEntries(context);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        private void DetachEntries(DbContext context)
        {
            foreach (var item in context.ChangeTracker.Entries<Ratings>())
            {
                item.State = EntityState.Detached;
            }
        }
    }

    public sealed class RatingMap : ClassMap<Ratings>
    {
        private static readonly DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public RatingMap()
        {
            Map(x => x.UserId).Name("userId");
            Map(x => x.MovieId).Name("movieId");
            Map(x => x.Rating).Name("rating");
            Map(x => x.Date).ConvertUsing(row => date.AddSeconds(double.Parse(row.GetField("timestamp"))));
        }
    }
}
