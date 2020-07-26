using CsvHelper;
using CsvHelper.Configuration;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SeedMovieLens.CsvService
{
    public class RatingsService
    {
        public void ReadCsvFile(string location, DbContext context)
        {
            try
            {
                using var reader = new StreamReader(location, Encoding.UTF8);
                using var csv = new CsvReader(reader: reader, CultureInfo.InvariantCulture);
                csv.Configuration.RegisterClassMap<RatingMap>();
                var rating = new Ratings();
                foreach (var r in csv.EnumerateRecords(rating))
                {
                    context.Entry(r).State = EntityState.Added;
                    context.SaveChanges();
                    context.Entry(r).State = EntityState.Detached;
                }
            }
            catch (Exception)
            {
                throw;
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
