using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class ManageViewModel
    {
        public IEnumerable<Manage> Manages { get; set; }
        public string Search { get; set; }
    }
}