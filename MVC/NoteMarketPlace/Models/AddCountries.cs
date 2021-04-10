using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class AddCountries
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Country Name *")]
        public string CountryName { get; set; }

        [Required]
        [Display(Name = "Country Code *")]
        public string CountryCode { get; set; }
    }
}