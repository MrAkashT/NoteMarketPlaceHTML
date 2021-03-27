using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class SearchNoteWrap
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string DisplayPicture { get; set; }
        public int SellerID { get; set; }
        public string UniversityName { get; set; }
        public int? NumberOfPages { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Category { get; set; }
        public int? NoteType { get; set; }
        public string Course { get; set; }
        public int? Country { get; set; }
        public decimal avg { get; set; }
        public int count { get; set; }
        public int inappropriateCount { get; set; }
    }
}