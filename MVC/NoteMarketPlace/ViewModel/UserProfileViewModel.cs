//using NoteMarketPlace.Entities;
using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class UserProfileViewModel
    {
        public UserProfileDetails User { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<ReferenceData> Gender { get; set; }

    }
}