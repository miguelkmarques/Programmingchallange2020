using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
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

        public void ReadCsvFile(string location, DbContext context, string connectionString)
        {
            OptionsBuilder.UseNpgsql(connectionString);
            try
            {
                int countRecords = 0;
                //string templateInsertString = "insert into ratings values (@UserId, @MovieId, @Rating, @Date)";
                using (var reader = new StreamReader(location, Encoding.UTF8))
                {
                    while (reader.ReadLine() != null)
                    {
                        countRecords++;
                    }
                    countRecords--;
                }
                int divisao = countRecords / 4;

                List<Task> tasks = new List<Task>
                {
                    Task.Run(() => ReadPartialCsv(location, new ApplicationDbContext(OptionsBuilder.Options), divisao * 0, divisao)),
                    Task.Run(() => ReadPartialCsv(location, new ApplicationDbContext(OptionsBuilder.Options), divisao * 1, divisao)),
                    Task.Run(() => ReadPartialCsv(location, new ApplicationDbContext(OptionsBuilder.Options), divisao * 2, divisao)),
                    Task.Run(() => ReadPartialCsv(location, new ApplicationDbContext(OptionsBuilder.Options), divisao * 3, countRecords - divisao * 3)),
                };


                Task.WaitAll(tasks.ToArray());

                //using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                //csv.Configuration.RegisterClassMap<RatingMap>();
                //int count = 0;
                //foreach (var r in csv.GetRecords<Ratings>())
                //{
                //    context.Add(r);
                //    if (count == 1000)
                //    {
                //        context.SaveChanges();
                //        DetachEntries(context);
                //        count = 0;
                //    }
                //    count++;
                //}
                //context.SaveChanges();
                //DetachEntries(context);

                //foreach (var r in csv.EnumerateRecords(rating))
                //{
                //var parameters = new DynamicParameters();
                //parameters.Add("@UserId", r.UserId, DbType.Int64, ParameterDirection.Input);
                //parameters.Add("@MovieId", r.MovieId, DbType.Int64, ParameterDirection.Input);
                //parameters.Add("@Rating", r.Rating, DbType.Decimal, ParameterDirection.Input);
                //parameters.Add("@Date", r.Date, DbType.DateTime2, ParameterDirection.Input);
                //rows.Add(parameters);
                //context.Entry(r).State = EntityState.Added;
                //context.SaveChanges();
                //context.Entry(r).State = EntityState.Detached;
                //}

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReadPartialCsv(string location, DbContext context, int skip, int take)
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
        private static DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public RatingMap()
        {
            Map(x => x.UserId).Name("userId");
            Map(x => x.MovieId).Name("movieId");
            Map(x => x.Rating).Name("rating");
            Map(x => x.Date).ConvertUsing(row => date.AddSeconds(double.Parse(row.GetField("timestamp"))));
        }
    }
}
