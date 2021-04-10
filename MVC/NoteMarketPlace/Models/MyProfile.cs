using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class MyProfile
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name *")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name *")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email *")]
        public string Email { get; set; }
        

        [EmailAddress]
        [Display(Name = "Secondary Email *")]
        public string SecondaryEmail { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase AdminImage { get; set; }

        [Required]
        [Display(Name = "Phone Number *")]
        public string CountryCodeId { get; set; }
    }
}