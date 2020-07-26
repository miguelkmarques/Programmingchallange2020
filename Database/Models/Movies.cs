using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Movies
    {
        public long MovieId { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public decimal AverageRating { get; set; }

        public List<MovieGenres> Genres { get; set; }

        public List<Ratings> Ratings { get; set; }

        public Links Links { get; set; }

        public List<Tags> Tags { get; set; }

        public List<GenomeScores> GenomeScores { get; set; }
    }
}
