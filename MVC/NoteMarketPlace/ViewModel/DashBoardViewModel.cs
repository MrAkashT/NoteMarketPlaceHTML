using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class DashBoardViewModel
    {
        
        public List<DashBoard> Notes { get; set; }
        public List<DashBoard> publishedNotes { get; set; }
        public string SearchDraftNote { get; set; }
        public string SearchPublishNote { get; set; }
        public int BuyerCount { get; set; }
        public int RejectedCount { get; set; }
        public decimal MoneyEarned { get; set; }
        public int MyDownload { get; set; }
        public int NumberOfSold { get; set; }

    }
}