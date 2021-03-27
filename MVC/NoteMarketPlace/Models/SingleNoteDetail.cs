using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class SingleNoteDetail
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public SellerNote sellerNote { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string BuyerName { get; set; }
        public decimal avg { get; set; }
        public int count { get; set; }
        public int inappropriateCount { get; set; }
        public IEnumerable<SingleReview> Reviews { get; set; }

    }
}