using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class MovieGenres
    {
        public long MovieId { get; set; }

        public Movies Movie { get; set; }

        public string Genre { get; set; }
    }
}
