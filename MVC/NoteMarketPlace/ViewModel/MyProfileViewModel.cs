using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class MyProfileViewModel
    {
        public MyProfile Admin { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}