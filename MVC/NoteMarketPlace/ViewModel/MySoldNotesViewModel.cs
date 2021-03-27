using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class MySoldNotesViewModel
    {
        public IEnumerable<MyTable> MySoldNotes { get; set; }
        public string search { get; set; }
    }
}