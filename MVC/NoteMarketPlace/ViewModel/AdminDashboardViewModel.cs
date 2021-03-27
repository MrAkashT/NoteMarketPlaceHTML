using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class AdminDashboardViewModel
    {
        public int PublishedCount { get; set; }
        public int NumberOfDownload { get; set; }
        public int NumberOfReg { get; set; }
        public IEnumerable<AdminPublishedNotes> PublishedNotes { get; set; }

        public string searchNote { get; set; }
        public IEnumerable<string> LastSixMonth { get; set; }
        public string month { get; set; }
    }
}