//using NoteMarketPlace.Entities;
using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class NoteDetailsViewModel
    {
        public NoteDetails NoteDetails { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<NoteCategory> Categories { get; set; }
        public IEnumerable<NoteType> Types { get; set; }
        [Required]
        public IEnumerable<ReferenceData> SellingMode { get; set; }

    }
}