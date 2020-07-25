using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class GenomeScores
    {
        public long MovieId { get; set; }

        public long TagId { get; set; }

        public decimal Relevance { get; set; }

        public Movies Movie { get; set; }

        public GenomeTags GenomeTag { get; set; }
    }
}
