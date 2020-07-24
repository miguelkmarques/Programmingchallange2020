using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Ratings
    {
        public long UserId { get; set; }

        public long MovieId { get; set; }

        public decimal Rating { get; set; }

        public DateTime Date { get; set; }

        public Movies Movie { get; set; }
    }
}
