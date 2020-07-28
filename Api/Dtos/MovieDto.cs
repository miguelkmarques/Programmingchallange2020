using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class MovieDto
    {
        public long movieId { get; set; }

        public string title { get; set; }

        public int year { get; set; }

        public decimal averageRating { get; set; }

        public List<GenreDto> genres { get; set; }
    }

    public class GenreDto
    {
        public string genre { get; set; }
    }


}
