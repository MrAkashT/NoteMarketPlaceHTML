using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class BuyerReq
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Buyer { get; set; }
        public string code { get; set; }
        public string PhoneNo { get; set; }
        public string SellType { get; set; }
        public bool isPaid { get; set; }
        public decimal? Price { get; set; }
        [DisplayFormat(DataFormatString  = "d:0 MMM yyyy")]
        public DateTime downloadDate { get; set; }
    }
}