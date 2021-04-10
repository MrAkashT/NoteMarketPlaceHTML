using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class SingleReview
    {
        public int Id { get; set; }
        public string pic { get; set; }
        public int SellerId { get; set; }
        public string Name { get; set; }
        public string comments { get; set; }
        public decimal rating { get; set; }

    }
}