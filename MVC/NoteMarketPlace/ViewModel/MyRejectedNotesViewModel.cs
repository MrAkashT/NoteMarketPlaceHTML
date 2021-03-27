using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class MyRejectedNotesViewModel
    {
        public IEnumerable<MyTable> RejectedNotes { get; set; }
        public string search { get; set; }
    }
}