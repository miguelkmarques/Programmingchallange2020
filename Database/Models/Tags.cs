using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Tags
    {
        public long TagId { get; set; }

        public long UserId { get; set; }

        public long MovieId { get; set; }

        public string Tag { get; set; }

        public DateTime Date { get; set; }

        public Movies Movie { get; set; }
    }
}
