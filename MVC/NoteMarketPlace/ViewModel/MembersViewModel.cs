using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class MembersViewModel
    {
        public IEnumerable<Members> Members { get; set; }
        public string Search { get; set; }
    }
}