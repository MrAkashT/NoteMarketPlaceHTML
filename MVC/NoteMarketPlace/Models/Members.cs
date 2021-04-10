using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class Members
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime JoiningDate { get; set; }
        public int UnderReviewNotesCount { get; set; }
        public int PublishedNotesCount { get; set; }
        public int DownloadedNotesCount { get; set; }
        public decimal TotalExpensive { get; set; }
        public decimal TotalEarning { get; set; }
    }
}