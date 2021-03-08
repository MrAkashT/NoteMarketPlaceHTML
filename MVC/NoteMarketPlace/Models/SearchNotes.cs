using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class SearchNotes
    {
        public IEnumerable<SellerNote> sellerNotes { get; set; }
        public IEnumerable<NoteCategory> Categories { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<NoteType> Types { get; set; }
        public List<string> Universities { get; set; }
        public List<string> Courses { get; set; }
        public IEnumerable<SellerNote> Ratings { get; set; }
        public int UniversityId { get; set; }
        public int CourseId { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public int CountryId { get; set; }
    }
}