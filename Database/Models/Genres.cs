using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Genres
    {
        public string Genre { get; set; }

        public List<MovieGenres> Movies { get; set; }
    }
}
