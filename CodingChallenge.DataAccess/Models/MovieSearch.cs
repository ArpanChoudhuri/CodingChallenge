using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge.DataAccess.Models
{
    public class MovieSearch
    {
        public string Title { get; set; }

        public double? RatingAbove { get; set; }
        public double? RatingBelow { get; set; }

        public string Franchise { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
