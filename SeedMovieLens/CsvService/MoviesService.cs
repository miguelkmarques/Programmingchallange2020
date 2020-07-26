using CsvHelper;
using CsvHelper.Configuration;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SeedMovieLens.CsvService
{
    public class MoviesService
    {
        public List<Movies> ReadCsvFile(string location)
        {
            try
            {
                using var reader = new StreamReader(location, Encoding.UTF8);
                using var csv = new CsvReader(reader: reader, CultureInfo.InvariantCulture);
                csv.Configuration.RegisterClassMap<MovieMap>();
                var records = csv.GetRecords<Movies>().ToList();
                return records;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public sealed class MovieMap : ClassMap<Movies>
    {
        public MovieMap()
        {
            Map(x => x.MovieId).Name("movieId");
            Map(x => x.Title).Name("title");
            Map(x => x.Genres).ConvertUsing(row => row.GetField("genres").Split("|").Select(s => new MovieGenres { Genre = s }).ToList());
        }
    }
}
