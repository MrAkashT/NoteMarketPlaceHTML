using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class Manage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
        public string Active { get; set; }
        public bool IsActive { get; set; }

    }
}