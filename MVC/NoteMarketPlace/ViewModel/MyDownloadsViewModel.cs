using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class MyDownloadsViewModel
    {
        public IEnumerable<MyTable> MyDownloads { get; set; }
        public string search { get; set; }
        public decimal rating { get; set; }
        public int downloadId { get; set; }
        [Required]
        public string comments { get; set; }
    }
}