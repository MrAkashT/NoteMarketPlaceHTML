using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using NoteMarketPlace.Entities;
using System.ComponentModel.DataAnnotations;

namespace NoteMarketPlace.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [Display(Name ="First Name *")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [Display(Name = "Last Name *")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is Required")]
        [Display(Name = "Email *")]
        [EmailAddress]
        public string EmailID { get; set; }
        
        [Required]
        [ForPassword]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string confirmPassword { get; set; }

    }
}