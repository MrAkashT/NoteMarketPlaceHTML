using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class AdminNotesViewModel
    {
        public IEnumerable<AdminPublishedNotes> PublishedNotes { get; set; }
        public IEnumerable<AdminTable> DownloadedNotes { get; set; }
        public IEnumerable<AdminTable> RejectedNotes { get; set; }
        public string Search { get; set; }
        public IEnumerable<User> Sellers { get; set; }
        public IEnumerable<User> Buyers { get; set; }
        public IEnumerable<string> NotesTitles { get; set; }
        public int Seller { get; set; }
        public int Buyer { get; set; }
        public string Note { get; set; }
    }
}