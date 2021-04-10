using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class SpamReportViewModel
    {
        public IEnumerable<AdminTable> Reports { get; set; }
        public string Search { get; set; }
    }
}