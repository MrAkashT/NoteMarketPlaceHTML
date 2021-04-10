using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class NotesUnderReviewViewModel
    {
        public IEnumerable<AdminTable> UnderReviewNotes { get; set; }
        public IEnumerable<User> Sellers { get; set; }
       // public string Seller { get; set; }
        public int Seller { get; set; }
        public string Search { get; set; }
    }
}