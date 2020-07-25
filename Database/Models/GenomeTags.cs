using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class GenomeTags
    {
        public long TagId { get; set; }

        public string Tag { get; set; }

        public List<GenomeScores> GenomeScores { get; set; }
    }
}
