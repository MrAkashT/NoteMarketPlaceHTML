using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "Name is Required.")]
        [Display(Name = "First Name *")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email Address is Required.")]
        [Display(Name = "Email *")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is Required.")]
        [Display(Name = "Subject *")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Comments or Questions is Required.")]
        [Display(Name = "Comments / Questions *")]
        public string Comments { get; set; }
    }
}