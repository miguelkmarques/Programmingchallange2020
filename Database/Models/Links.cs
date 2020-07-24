using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Links
    {
        public long MovieId { get; set; }

        public string ImdbId { get; set; }

        public long? TmdbId { get; set; }

        public Movies Movie { get; set; }
    }
}
