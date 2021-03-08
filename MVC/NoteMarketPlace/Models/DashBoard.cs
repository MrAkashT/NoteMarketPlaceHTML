using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class DashBoard
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public bool IsPaid { get; set; }
        public string SellType { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime AddedDate { get; set; }
        public string Status { get; set; }
       
    }
}