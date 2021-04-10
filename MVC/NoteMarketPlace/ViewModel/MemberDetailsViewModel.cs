using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class MemberDetailsViewModel
    {
        public IEnumerable<AdminTable> MemberNotes { get; set; }
        public MemberDetails Member { get; set; }
    }
}